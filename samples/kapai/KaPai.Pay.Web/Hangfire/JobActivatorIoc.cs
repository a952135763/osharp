using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace KaPai.Pay.Web.Hangfire
{
    public class JobActivatorIoc : JobActivator
    {

        readonly IServiceProvider _serviceProvider;

        public JobActivatorIoc(IServiceProvider service)
        {
            _serviceProvider = service;
        }

        public override object ActivateJob(Type jobType)
        {
            return _serviceProvider.GetService(jobType);
        }
    }
}
