using Seeger.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Seeger.Config
{
    public class SecurityConfig
    {
        public LoginMode SaLoginMode { get; private set; }

        public PermissionGroupCollection PermissionGroups { get; private set; }

        public SecurityConfig()
        {
            PermissionGroups = new PermissionGroupCollection();
        }

        public static SecurityConfig From(XElement element)
        {
            var settings = new SecurityConfig();

            var loginMode = element.AttributeValue("sa-login-mode");
            if (!String.IsNullOrEmpty(loginMode))
            {
                settings.SaLoginMode = (LoginMode)Enum.Parse(typeof(LoginMode), loginMode);
            }

            var permissionsElement = element.Element("permissions");
            if (permissionsElement != null)
            {
                foreach (var each in permissionsElement.Elements())
                {
                    var group = PermissionGroup.From(each, null);
                    settings.PermissionGroups.Add(group);
                }
            }

            return settings;
        }
    }
}
