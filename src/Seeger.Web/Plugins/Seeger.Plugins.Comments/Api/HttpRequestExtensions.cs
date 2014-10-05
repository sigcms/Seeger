using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments.Api
{
    static class HttpRequestExtensions
    {
        public static string GetIPAddress(this HttpRequest request)
        {
            var ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrWhiteSpace(ip))
            {
                ip = request.UserHostAddress;
            }

            return ip;
        }
    }
}