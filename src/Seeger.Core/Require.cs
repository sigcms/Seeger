using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public class Require
    {
        public static void That(bool condition, string message)
        {
            if (!condition)
            {
                throw new ArgumentException(message);
            }
        }

        public static void NotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotNullOrEmpty(string value, string parameterName)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException(String.Format("'{0}' is required.", parameterName));
            }
        }
    }
}
