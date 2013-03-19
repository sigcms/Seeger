using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Utils
{
    public static class ExceptionPrinter
    {
        public static string Print(Exception exception)
        {
            Require.NotNull(exception, "exception");

            var output = new StringBuilder();

            var current = exception;
            var first = true;

            while (current != null)
            {
                if (!first)
                {
                    output.AppendLine("=================================");
                }

                output.AppendLine(current.Message);
                output.AppendLine(current.StackTrace);

                first = false;
                current = current.InnerException;
            }

            return output.ToString();
        }
    }
}
