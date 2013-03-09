using Seeger.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Plugins
{
    public partial class PluginList : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List.DataSource = PluginManager.LoadedPlugins;
            List.DataBind();
        }

        protected void List_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsDataItem())
            {
                var plugin = (PluginDefinition)e.Item.DataItem;
                if (PluginManager.IsInstalled(plugin.Name))
                {
                    e.Item.FindControl("UninstallHolder").Visible = true;

                    if (PluginManager.IsEnabled(plugin.Name))
                    {
                        e.Item.FindControl("DisableHolder").Visible = true;
                    }
                    else
                    {
                        e.Item.FindControl("EnableHolder").Visible = true;
                    }
                }
                else
                {
                    e.Item.FindControl("InstallHolder").Visible = true;
                }
            }
        }

        [WebMethod, ScriptMethod]
        public static void Install(string pluginName)
        {
            PluginManager.InstallAndEnable(pluginName);
        }

        [WebMethod, ScriptMethod]
        public static void Uninstall(string pluginName)
        {
            PluginManager.Uninstall(pluginName);
        }

        [WebMethod, ScriptMethod]
        public static void Enable(string pluginName)
        {
            PluginManager.Enable(pluginName);
        }

        [WebMethod, ScriptMethod]
        public static void Disable(string pluginName)
        {
            PluginManager.Disable(pluginName);
        }
    }
}