using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Seeger
{
    public static class ObjectExtensions
    {
        public static string AsString(this object obj)
        {
            if (obj == null)
            {
                return String.Empty;
            }
            return obj.ToString();
        }
    }
}
