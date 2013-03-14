using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web
{
    public static class HttpContextExtensions
    {
        public static T GetOrAdd<T>(this HttpContext context, string key, Func<T> valueFactory)
        {
            var obj = context.Items[key];

            if (obj == null)
            {
                obj = valueFactory();
                context.Items[key] = obj;
            }

            return (T)obj;
        }
    }
}
