using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Seeger.Security;
using Seeger.Caching;
using Seeger.Globalization;
using Seeger.Templates;
using System.Web;

namespace Seeger.Web.UI
{
    public class AdminSession
    {
        public User User { get; private set; }

        public Skin Theme
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                if (User.Skin != null)
                {
                    return User.Skin;
                }

                return AdminSkins.Skins.First();
            }
        }

        public CultureInfo UICulture
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                if (User.Language != null)
                {
                    return CultureInfo.GetCultureInfo(User.Language);
                }

                var culture = ResourcesFolder.Global.Cultures.FirstOrDefault(c => c.Name == CultureInfo.CurrentUICulture.Name);
                if (culture == null)
                {
                    culture = ResourcesFolder.Global.Cultures.First();
                }

                return culture;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public AdminSession(User user)
        {
            User = user;
        }

        public static AdminSession Current
        {
            get
            {
                return HttpContext.Current.GetOrAdd<AdminSession>("Seeger.Web.AdminSession.Current", 
                    () => new AdminSession(AuthenticationService.GetCurrentUserFrom(HttpContext.Current.User)));
            }
        }
    }
}
