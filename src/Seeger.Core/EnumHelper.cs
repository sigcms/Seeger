using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public static class EnumHelper
    {
        public static TEnum Parse<TEnum>(string value)
            where TEnum : struct
        {
            return Parse<TEnum>(value, true);
        }

        public static TEnum Parse<TEnum>(string value, bool ignoreCase)
            where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
    }
}
