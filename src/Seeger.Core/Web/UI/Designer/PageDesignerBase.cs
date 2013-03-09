using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;

using Seeger.Security;
using Seeger.Globalization;
using Seeger.Plugins;

namespace Seeger.Web.UI
{
    public abstract class PageDesignerBase : LayoutPageBase, IPrivateResource
    {
        protected User CurrentUser
        {
            get
            {
                return AdministrationSession.User;
            }
        }

        protected AdminSession AdministrationSession
        {
            get { return AdminSession.Current; }
        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            if (AdministrationSession.IsAuthenticated)
            {
                var culture = AdministrationSession.UICulture;
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }

        protected override void SetupSeo()
        {
        }

        protected override void FixFromActionUrl()
        {
            // Don't fix action url in designer
        }

        protected override void SetupWidgets()
        {
            foreach (var block in PageItem.Layout.Zones)
            {
                var holder = block.LoadDesigner(this);
                if (holder != null)
                {
                    foreach (var widgetSetting in PageItem.GetWidgets(block.Name).OrderBy(it => it.Order))
                    {
                        var plugin = PluginManager.FindEnabledPlugin(widgetSetting.PluginName);
                        if (plugin == null) continue;

                        var widget = plugin.FindWidget(widgetSetting.WidgetName);

                        if (widget != null)
                        {
                            var designer = widget.LoadDesigner(this);
                            designer.WidgetInPageId = widgetSetting.Id.ToString();
                            designer.WidgetDisplayOrder = widgetSetting.Order;
                            designer.WidgetAttributes.AddRange(widgetSetting.Attributes);
                            holder.Controls.Add(designer);
                        }
                    }
                }
            }
        }

        protected override IList<string> GetThemeFilePaths()
        {
            IList<string> paths = base.GetThemeFilePaths();
            paths.Add(AdminSession.Current.Theme.GetFileVirtualPath("page-designer.css"));
            if (AdminSession.Current.Theme.ContainsFile("page-designer.css", CultureInfo.CurrentUICulture))
            {
                paths.Add(AdminSession.Current.Theme.GetFileVirtualPath("page-designer.css", CultureInfo.CurrentUICulture));
            }

            return paths;
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!AdministrationSession.IsAuthenticated)
            {
                AuthenticationService.RedirectToLoginPage();
            }
            else if (!CurrentUser.IsSuperAdmin && !VerifyAccess(CurrentUser))
            {
                AuthenticationService.RedirectToUnauthorizedPage();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Title = ResourcesFolder.Global.GetValue("Common.Designer", CultureInfo.CurrentUICulture) + " (" + PageItem.DisplayName + ")";

            IncludeScripts();
            IncludeDesignerElement();
        }

        private void IncludeDesignerElement()
        {
            Form.Controls.Add(LoadControl(CmsVirtualPath.GetFull("/Admin/Designer/DesignerElement.ascx")));
        }

        private void IncludeScripts()
        {
            Header.Controls.Add(new ScriptReference { Path = "/Scripts/jquery/jquery.min.js" });
            Header.Controls.Add(new ScriptReference { Path = "/Scripts/jquery/jquery-ui.min.js" });
        }

        public virtual bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "PageMgnt", "Design");
        }
    }

}
