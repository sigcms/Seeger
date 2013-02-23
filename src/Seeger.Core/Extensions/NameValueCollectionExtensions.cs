using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Seeger
{
    public static class NameValueCollectionExtensions
    {
        public static string TryGetValue(this NameValueCollection nv, string key, string defaultValue)
        {
            Require.NotNull(nv, "nv");
            Require.NotNullOrEmpty(key, "key");

            var value = nv[key];

            return value ?? defaultValue;
        }

        public static T TryGetValue<T>(this NameValueCollection nv, string key, T defaultValue)
        {
            Require.NotNull(nv, "nv");
            Require.NotNullOrEmpty(key, "key");

            var value = nv[key];

            if (value == null)
            {
                return defaultValue;
            }

            // TODO: Confusing, if url is ?xxx= , then TryGetValue("xxx", "0") return 0 or ""?
            //       0 is expected in this case but in other case, empty string IS a valid value (like FrontendSettings.PageExtension),
            //       so we might need to remove this method, and use methods like collection[key].DefaultIfNullOrEmpty("0") etc
            if (value.Length == 0)
            {
                return (T)(object)value;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
