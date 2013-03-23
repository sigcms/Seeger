using NHibernate.Linq;
using Seeger.Data;
using Seeger.Globalization;
using Seeger.Logging;
using Seeger.Security;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Seeger.Web
{
    public class AuthenticationService
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public static readonly int MaxInvalidPasswordAttempts = 5;

        public static readonly int PasswordAttemptWindowInMinutes = 30;

        public static readonly string PasswordHashAlgorithm = "SHA1";

        public static User Login(string userName, string password, string ip, bool persistCredential)
        {
            User user = null;

            try
            {
                user = Authenticate(userName, password, ip);
            }
            catch (Exception ex)
            {
                _log.Warn(UserReference.System(), String.Format("<t>User</t> {0} <t>login failed</t>. " + ex.Message, userName));
                throw;
            }

            if (user != null)
            {
                SetAuthCookie(user, persistCredential);
                _log.Info(UserReference.System(), String.Format("<t>User</t> {0} <t>login succeeded</t>", user.Nick + " (" + user.UserName + ")"));
            }

            return user;
        }

        public static User Authenticate(string userName, string password, string ip)
        {
            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException(ResourceFolder.Global.GetValue("Login.LoginFailed", CultureInfo.CurrentUICulture));

            var session = Database.GetCurrentSession();

            var user = session.Query<User>().FirstOrDefault(u => u.UserName == userName);

            if (user == null)
                throw new InvalidOperationException(ResourceFolder.Global.GetValue("Login.LoginFailed", CultureInfo.CurrentUICulture));

            if (user.FailedPasswordAttemptCount >= MaxInvalidPasswordAttempts)
            {
                var unlockTime = user.LastFailedPasswordAttemptTime.Value.AddMinutes(PasswordAttemptWindowInMinutes);

                if (DateTime.Now < unlockTime)
                {
                    throw new InvalidOperationException(ResourceFolder.Global.GetValue("Login.LockedForTooManyInvalidPasswordAttempts", CultureInfo.CurrentUICulture));
                }
            }

            if (user.Password != HashPassword(password))
            {
                user.LastFailedPasswordAttemptTime = DateTime.Now;
                user.FailedPasswordAttemptCount++;

                session.Commit();

                throw new InvalidOperationException(ResourceFolder.Global.GetValue("Login.LoginFailed", CultureInfo.CurrentUICulture));
            }
            else
            {
                user.LastLoginIP = ip;
                user.LastLoginTime = DateTime.Now;
                user.FailedPasswordAttemptCount = 0;
            }

            session.Commit();

            return user;
        }

        public static string HashPassword(string password)
        {
            Require.NotNullOrEmpty(password, "password");

            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, PasswordHashAlgorithm);
        }

        public static User GetCurrentUserFrom(IPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                string userName = ExtractUserName(principal.Identity.Name);

                if (!String.IsNullOrEmpty(userName))
                {
                    return Database.GetCurrentSession().Query<User>().FirstOrDefault(it => it.UserName == userName);
                }
            }

            return null;
        }

        public static void RedirectToLoginPage()
        {
            string loginUrl = FormsAuthentication.LoginUrl;
            if (String.IsNullOrEmpty(loginUrl))
            {
                loginUrl = "/Admin/Login.aspx";
            }

            HttpContext.Current.Response.Redirect(loginUrl, true);
        }

        public static void RedirectToUnauthorizedPage()
        {
            HttpContext.Current.Response.Redirect("/Admin/403.aspx", true);
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
