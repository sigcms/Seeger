using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Utils
{
    public static class JsonConvertUtil
    {
        public static string CamelCaseSerializeObject(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(value, Formatting.None, settings);
        }
    }
}
