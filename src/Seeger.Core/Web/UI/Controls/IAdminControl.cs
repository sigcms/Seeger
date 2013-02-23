using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Security;
using Seeger.Licensing;

namespace Seeger.Web.UI
{
    public interface IAdminControl
    {
        bool RequireSuperAdmin { get; }

        string Feature { get; }

        string Plugin { get; }

        string PermissionGroup { get; }

        string Permission { get; }
    }

    public static class IAdminControlExtensions
    {
        public static bool ValidateAccess(this IAdminControl control, User user)
        {
            Require.NotNull(control, "control");

            if (user == null)
            {
                return false;
            }

            string feature = control.Feature;

            if (!String.IsNullOrEmpty(feature) && !LicensingService.CurrentLicense.CmsEdition.IsFeatureAvailable(feature))
            {
                return false;
            }

            if (control.RequireSuperAdmin && user.IsSuperAdmin == false)
            {
                return false;
            }

            string pluginName = control.Plugin;
            string function = control.PermissionGroup;
            string operation = String.IsNullOrEmpty(control.Permission) ? "View" : control.Permission;

            if (!String.IsNullOrEmpty(function) && !user.HasPermission(pluginName, function, operation))
            {
                return false;
            }

            return true;
        }
    }
}
