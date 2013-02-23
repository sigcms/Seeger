using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Seeger.Web.UI
{
    public static class StateBagExtensions
    {
        public static object TryGetValue(this StateBag viewState, string key, object defaultValue)
        {
            object value = viewState[key];
            if (value == null)
            {
                return defaultValue;
            }
            return value;
        }

        public static T TryGetValue<T>(this StateBag viewState, string key, T defaultValue)
        {
            object value = viewState[key];
            if (value == null)
            {
                return defaultValue;
            }
            if (value is T)
            {
                return (T)value;
            }
            throw new InvalidOperationException(String.Format("Value cannot be cast to type {0}.", typeof(T).FullName));
        }
    }
}
