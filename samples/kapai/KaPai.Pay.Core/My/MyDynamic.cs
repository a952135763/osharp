using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using OSharp.Extensions;
using OSharp.Json;

namespace KaPai.Pay.My
{

    public class MyDynamic : DynamicObject
    {

        private readonly ConcurrentDictionary<string, object> _values;

        public MyDynamic(ConcurrentDictionary<string, object> dictionary = null) : base()
        {
            _values = dictionary ?? new ConcurrentDictionary<string, object>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _values.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _values[binder.Name] = value;
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var str = JsonHelper.ToJson(_values);
            str = JsonHelper.JsonDateTimeFormat(str);
            result = JsonConvert.DeserializeObject(str, binder.Type);
            return true;
        }


        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _values.Keys;
        }

        public object this[string index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

    }
}
