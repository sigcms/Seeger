using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

using Seeger.Data;
using Seeger.Security;
using NHibernate.Linq;

namespace Seeger.Web
{
    public class AuthenticationService
    {
        public static readonly string PasswordHashAlgorithm = "SHA1";

        public static User Login(string userName, string password, bool persistCredential)
        {
            User user = Authenticate(userName, password);
            if (user != null)
            {
                SetAuthCookie(user, persistCredential);
            }

            return user;
        }

        public static User Authenticate(string userName, string password)
        {
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                string hashedPassword = HashPassword(password);

                User user = NhSessionManager.GetCurrentSession().Query<User>().FirstOrDefault(it =>it.UserName == userName && it.Password == hashedPassword);

                return user;
            }

            return null;
        }

        public static string HashPassword(string password)
        {
            Require.NotNullOrEmpty(password, "password");

            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, PasswordHashAlgorithm);
        }

        public static User GetCurrentUserFromCookie()
        {
            var principal = HttpContext.Current.User;

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                string userName = ExtractUserName(principal.Identity.Name);

                if (!String.IsNullOrEmpty(userName))
                {
                    return NhSessionManager.GetCurrentSession().Query<User>().FirstOrDefault(it => it.UserName == userName);
                }
            }

            return null;
        }

        public static void RedirectToLoginPage()
        {
            string loginUrl = FormsAuthentication.LoginUrl;
            if (String.IsNullOrEmpty(loginUrl))
            {
                loginUrl = CmsVirtualPath.GetFull("/Admin/Login.aspx");
            }

            HttpContext.Current.Response.Redirect(loginUrl, true);
        }

        public static void RedirectToUnauthorizedPage()
        {
            HttpContext.Current.Response.Redirect(CmsVirtualPath.GetFull("/Admin/403.aspx"), true);
        }

        public static void SetAuthCookie(User user, bool persistCredential)
        {
            Require.NotNull(user, "user");

            FormsAuthentication.SetAuthCookie(GetAuthCookieName(user), persistCredential);
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        private static string GetAuthCookieName(User user)
        {
            return "[SEEGER]" + user.UserName;
        }

        private static string ExtractUserName(string cookieName)
        {
            if (cookieName.StartsWith("[SEEGER]") && cookieName.Length > "[SEEGER]".Length)
            {
                return cookieName.Substring("[SEEGER]".Length);
            }

            return null;
        }
    }
}
