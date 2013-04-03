using Seeger.Security;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Seeger.Web
{
    public enum HandlerAuthMode
    {
        AuthByCookie = 0,
        AuthByPostedToken = 1
    }

    public abstract class AuthRequiredHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public virtual string AuthTokenName
        {
            get
            {
                return "AspNetAuth";
            }
        }

        public virtual HandlerAuthMode AuthMode
        {
            get
            {
                return HandlerAuthMode.AuthByCookie;
            }
        }

        public AdminSession AdminSession { get; private set; }

        public void ProcessRequest(HttpContext context)
        {
            AdminSession = Authenticate(context);

            if (AdminSession.IsAuthenticated && (AdminSession.User.IsSuperAdmin) || Authorize(AdminSession.User))
            {
                DoProcessRequest(context);
            }
            else
            {
                OnAccessDenied(context);
            }
        }

        protected virtual AdminSession Authenticate(HttpContext context)
        {
            if (AuthMode == HandlerAuthMode.AuthByCookie)
            {
                return AdminSession.Current;
            }

            var token = context.Request.Params[AuthTokenName];

            if (token != null)
            {
                var ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null)
                {
                    var identity = new FormsIdentity(ticket);
                    var principal = new GenericPrincipal(identity, new string[] { });
                    context.User = principal;
                }
            }

            var user = AuthenticationService.GetCurrentUserFrom(context.User);

            return new AdminSession(user);
        }

        protected virtual bool Authorize(User user)
        {
            return true;
        }

        protected abstract void DoProcessRequest(HttpContext context);

        protected virtual void OnAccessDenied(HttpContext context)
        {
            context.Response.StatusCode = 403;
        }
    }
}
