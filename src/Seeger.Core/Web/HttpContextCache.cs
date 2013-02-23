using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web
{
    public class HttpContextCache
    {
        public static T GetObject<T>(string key, Func<T> factory)
        {
            var context = HttpContext.Current;

            var obj = context.Items[key];

            if (obj == null)
            {
                obj = factory();
                context.Items[key] = obj;
            }

            return (T)obj;
        }
    }
}
