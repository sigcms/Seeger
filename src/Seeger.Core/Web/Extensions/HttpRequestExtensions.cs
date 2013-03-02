using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Seeger.Web
{
    public static class HttpRequestExtensions
    {
        public static string GetAuthCookieValue(this HttpRequest request)
        {
            return request.Cookies[FormsAuthentication.FormsCookieName] == null ?
                string.Empty
                :
                request.Cookies[FormsAuthentication.FormsCookieName].Value;
        }
    }
}
