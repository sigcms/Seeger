using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Security;
using Seeger.Plugins;

namespace Seeger.Web.UI.Admin.Security
{
    public partial class PrivilegeView : System.Web.UI.UserControl
    {
        private Role _role;

        private User CurrentUser
        {
            get { return AdministrationSession.Current.User; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Bind(Role role)
        {
            _role = role;

            IEnumerable<PermissionGroup> functions = CmsConfiguration.Instance.PermissionGroups;
            var plugins = PluginManager.EnabledPlugins;

            foreach (var plugin in plugins)
            {
                functions = functions.Union(plugin.PermissionGroups);
            }

            FunctionRepeater.DataSource = functions;
            FunctionRepeater.DataBind();
        }

        protected void FunctionRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsDataItem())
            {
                var group = (PermissionGroup)e.Item.DataItem;
                var pluginName = group.Plugin == null ? String.Empty : group.Plugin.Name;

                ViewState["PluginName" + e.Item.ItemIndex] = pluginName;
                ViewState["GroupName" + e.Item.ItemIndex] = group.Name;

                var chbList = (CheckBoxList)e.Item.FindControl("Operations");

                foreach (var each in group.Permissions.OrderBy(it => it.Weight))
                {
                    ListItem item = new ListItem(each.DisplayName.Localize(), each.Name);
                    chbList.Items.Add(item);

                    item.Selected = _role.HasPermission(pluginName, group.Name, each.Name);
                    item.Attributes.Add("weight", each.Weight.ToString());

                    if (!CurrentUser.HasPermission(pluginName, group.Name, each.Name))
                    {
                        item.Enabled = false;
                    }
                }
            }
        }

        public void Update(Role role)
        {
            Require.NotNull(role, "role");

            foreach (RepeaterItem item in FunctionRepeater.Items)
            {
                if (item.IsDataItem())
                {
                    string pluginName = ViewState["PluginName" + item.ItemIndex].ToString();
                    string groupName = ViewState["GroupName" + item.ItemIndex].ToString();

                    PermissionGroup group = null;

                    if (!String.IsNullOrEmpty(pluginName))
                    {
                        var plugin = PluginManager.FindEnabledPlugin(pluginName);
                        if (plugin != null)
                        {
                            group = plugin.PermissionGroups.Find(groupName);
                        }
                    }
                    else
                    {
                        group = CmsConfiguration.Instance.PermissionGroups.Find(groupName);
                    }

                    if (group == null) continue;

                    var chbList = (CheckBoxList)item.FindControl("Operations");

                    var selectedOperations = new List<Permission>();
                    var unselectedOperations = new List<Permission>();

                    foreach (ListItem each in chbList.Items)
                    {
                        if (each.Selected)
                        {
                            selectedOperations.Add(group.Permissions.Find(each.Value));
                        }
                        else
                        {
                            unselectedOperations.Add(group.Permissions.Find(each.Value));
                        }
                    }

                    foreach (var each in selectedOperations)
                    {
                        role.AddPermission(each);
                    }

                    foreach (var each in unselectedOperations)
                    {
                        role.RemovePermission(each);
                    }
                }
            }
        }
    }
}