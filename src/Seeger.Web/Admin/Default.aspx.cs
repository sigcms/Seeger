using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;

using Seeger.Licensing;
using Seeger.Security;
using Seeger.Plugins;

namespace Seeger.Web.UI.Admin
{
    public partial class Default : Seeger.Web.UI.AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string RenderAdminMenu()
        {
            XmlMenu menu = XmlMenu.AdminMenu;

            StringBuilder html = new StringBuilder();
            foreach (var section in menu.Items)
            {
                var items = section.Items.Where(it => it.ValidateAccess(CurrentUser) == true);

                if (items.Any())
                {
                    html.Append("<ul>");

                    foreach (var item in items)
                    {
                        html.AppendFormat("<li><a href='{0}' target='content-iframe'>{1}</a></li>", item.NavigateUrl, item.Title.Localize());
                    }

                    html.Append("</ul>");
                }
            }

            return html.ToString();
        }

        protected string RenderModuleMenus()
        {
            var plugins = PluginManager.EnabledPlugins;
            if (!plugins.Any())
            {
                return String.Empty;
            }

            StringBuilder html = new StringBuilder();

            foreach (var module in plugins)
            {
                var items = module.Menu.Items.Where(it => it.ValidateAccess(CurrentUser) == true);

                if (items.Any())
                {
                    html.AppendFormat("<h3 class='section-title'>{0}</h3>", module.DisplayName.Localize());

                    html.Append("<ul>");

                    foreach (var item in items)
                    {
                        html.AppendFormat("<li><a target='content-iframe' href='{0}'>{1}</a></li>", item.NavigateUrl, item.Title);
                    }

                    html.Append("</ul>");
                }
            }

            return html.ToString();
        }
    }
}