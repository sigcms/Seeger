using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web;

using Seeger.Caching;

namespace Seeger.Web
{
    public class FrontendUrlUtility
    {
        public static string EnsureCultureSegment(string rawPath, CultureInfo culture)
        {
            if (rawPath.Length == 0 || rawPath == "/")
            {
                return "/" + culture.Name;
            }

            rawPath = Normalize(rawPath);

            if (!rawPath.IgnoreCaseStartsWith("/" + culture.Name))
            {
                rawPath = "/" + culture.Name + rawPath;
            }

            return rawPath;
        }

        public static string Normalize(string rawPath)
        {
            if (String.IsNullOrEmpty(rawPath))
            {
                return "/";
            }

            if (!rawPath.StartsWith("/"))
            {
                return "/" + rawPath;
            }

            return rawPath;
        }
    }
}
