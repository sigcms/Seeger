using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Plugins.Comments
{
    public interface IAuthenticationContext
    {
        bool IsAuthenticated(HttpContextBase httpContext);

        UserInfo GetCurrentUser(HttpContextBase httpContext);
    }

    public static class AuthenticationContexts
    {
        public static IAuthenticationContext Current = new DefaultAuthenticationContext();
    }
}
