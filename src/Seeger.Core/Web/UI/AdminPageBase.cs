using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

using Seeger.Globalization;
using Seeger.Data;
using Seeger.Security;
using Seeger.Licensing;

namespace Seeger.Web.UI
{
    public abstract class AdminPageBase : PageBase, IPrivateResource
    {
        public User CurrentUser
        {
            get
            {
                return AdminSession.User;
            }
        }

        public AdminSession AdminSession
        {
            get { return AdminSession.Current; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!AdminSession.IsAuthenticated)
            {
                AuthenticationService.RedirectToLoginPage();
            }
            else if (!CurrentUser.IsSuperAdmin && !VerifyAccess(CurrentUser))
            {
                AuthenticationService.RedirectToUnauthorizedPage();
            }
        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            if (AdminSession.IsAuthenticated)
            {
                var culture = AdminSession.UICulture;
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string html = HtmlHelper.LinkCssFiles(GetCssFilePaths());

            if (!String.IsNullOrEmpty(html))
            {
                Literal ctrl = new Literal();
                Header.Controls.Add(ctrl);
                ctrl.Text = html;
            }

            LocalizeTitle();
        }

        private void LocalizeTitle()
        {
            if (String.IsNullOrEmpty(Title))
            {
                return;
            }

            var descriptor = ResourceDescriptor.Parse(Title);
            var localizer = SmartLocalizer.GetForCurrentRequest();

            if (String.IsNullOrEmpty(descriptor.PluginName))
            {
                descriptor.PluginName = localizer.PluginName;
            }
            if (String.IsNullOrEmpty(descriptor.TemplateName) && !String.IsNullOrEmpty(localizer.TemplateName))
            {
                descriptor.TemplateName = localizer.TemplateName;
            }
            if (String.IsNullOrEmpty(descriptor.WidgetName) && !String.IsNullOrEmpty(localizer.WidgetName))
            {
                descriptor.WidgetName = localizer.WidgetName;
            }

            Title = descriptor.Localize(CultureInfo.CurrentUICulture);
        }

        protected virtual IEnumerable<string> GetCssFilePaths()
        {
            return AdminSession.Theme.GetCssFileVirtualPaths(CultureInfo.CurrentUICulture);
        }

        protected string Localize(string key)
        {
            return Localize(key, CultureInfo.CurrentUICulture);
        }

        protected virtual string Localize(string key, CultureInfo culture)
        {
            return SmartLocalizer.GetForCurrentRequest().Localize(key, culture);
        }

        public virtual bool VerifyAccess(User user)
        {
            return true;
        }
    }
}
