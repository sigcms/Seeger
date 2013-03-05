using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Seeger
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            var field = @enum.GetType().GetField(@enum.ToString());
            var attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }

            return @enum.ToString();
        }
    }
}
