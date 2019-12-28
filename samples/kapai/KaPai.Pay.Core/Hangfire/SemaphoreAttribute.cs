using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire.Common;
using Hangfire.States;
using Hangfire;
using JetBrains.Annotations;

namespace KaPai.Pay.Hangfire
{
    /// <summary>
    /// 限制 Hangfire 并发数
    /// </summary>
    public class SemaphoreAttribute : JobFilterAttribute, IElectStateFilter
    {

        protected string Structs { get; set; }

        protected int Timeout { get; set; }

        protected int Max { get; set; }

        /// <summary>
        /// 限制任务同时处理数量 注意:并发键 和 同时最大数 设置的时候 不要同一个并发键 配置多个最大运行数
        /// </summary>
        /// <param name="name">并发键 使用'{0}' 表示参数也参与并发键生成</param>
        /// <param name="maxnumb">同时最大运行</param>
        /// <param name="outime">超时时间</param>
        public SemaphoreAttribute([NotNull]string name, int maxnumb = 1, int timeout = 5000)
        {

            Structs = name;
            Max = maxnumb;
            Timeout = timeout;
        }


        public void OnStateElection(ElectStateContext filterContext)
        {
            // 增加一个标志  标识此任务是否已分配等待任务
            if (filterContext.GetJobParameter<int>("AlreadyAssigned") > 0) return;
            if (string.IsNullOrEmpty(filterContext.CurrentState) && filterContext.CandidateState.Name == "Enqueued")  // 默认创建任务
            {
                
                ConcurrentHandle(filterContext);
            }
            else if (filterContext.CurrentState == "Scheduled" && filterContext.CandidateState.Name == "Enqueued")   // 定时入队任务
            {
                
                // 时间已经到了  但是还是得限制最大运行
                ConcurrentHandle(filterContext);
            }
            else
            {
                // Console.WriteLine($"{filterContext.BackgroundJob.Id}-{filterContext.CurrentState}-{filterContext.CandidateState.Name}");
            }

        }



        protected void ConcurrentHandle(ElectStateContext filterContext)
        {
            filterContext.SetJobParameter("AlreadyAssigned", 1);
            // 创建任务 分布式锁
            IDisposable blockbuster = null;
            try
            {
                var str = string.Format(Structs, filterContext.BackgroundJob.Job.Args.ToArray());

                // 进入分布式锁
                blockbuster = filterContext.Connection.AcquireDistributedLock($"concurrent:{str}:lock", TimeSpan.FromSeconds(Timeout));
                var recount = filterContext.Connection.GetAllEntriesFromHash($"concurrent:{str}:count");
                var array = filterContext.Connection.GetAllEntriesFromHash($"concurrent:{str}:array");
                int count = 0;
                if (recount == null)
                {
                    recount = new Dictionary<string, string>();
                    recount.TryAdd("recount", count.ToString());
                }
                else
                {
                    if (recount.TryGetValue("recount", out string o))
                    {
                        count = Convert.ToInt32(o);
                    }
                }
                if (array == null) array = new Dictionary<string, string>();
                count++;
                recount["recount"] = count.ToString();
                var y = count % Max;
                if (array.TryGetValue(y.ToString(), out string pId))
                {
                    // 防止自己等待自己
                    if (pId != filterContext.BackgroundJob.Id)
                    {
                        // 获取父任务的运行状态  
                        var pState = filterContext.Connection.GetJobParameter(pId, "State");
                        // 如果状态不是获取不到(获取不到表示没这任务) 则等待
                        if (!string.IsNullOrEmpty(pState))
                        { 
                            filterContext.CandidateState = new AwaitingState(pId, new EnqueuedState(), JobContinuationOptions.OnlyOnSucceededState);
                        }
                    }
                    array[y.ToString()] = filterContext.BackgroundJob.Id;
                }
                else
                {
                    array.TryAdd(y.ToString(), filterContext.BackgroundJob.Id);
                }
                filterContext.Transaction.SetRangeInHash($"concurrent:{str}:count",recount);
                filterContext.Transaction.SetRangeInHash($"concurrent:{str}:array",array);
                filterContext.Transaction.Commit();
            }
            catch (Exception e)
            {

                // 
            }
            blockbuster?.Dispose();
        }

    }





}
