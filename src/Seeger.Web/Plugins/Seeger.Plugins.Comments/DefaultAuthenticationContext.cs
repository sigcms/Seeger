using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Seeger.Plugins.Comments
{
    public class DefaultAuthenticationContext : IAuthenticationContext
    {
        public bool IsAuthenticated(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated;
        }

        public UserInfo GetCurrentUser(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return new UserInfo
                {
                    Id = httpContext.User.Identity.Name,
                    Nick = httpContext.User.Identity.Name
                };
            }

            return null;
        }
    }
}