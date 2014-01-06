using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using Seeger.Plugins;
using Seeger.Config;
using Seeger.Data.Mapping;
using Seeger.ComponentModel;

namespace Seeger.Security
{
    [Entity]
    public class Role
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<RoleGrantedPermission> GrantedPermissions { get; protected set; }

        public Role()
        {
            GrantedPermissions = new List<RoleGrantedPermission>();
        }

        public virtual bool HasPermission(string pluginName, string groupName, string permissionName)
        {
            if (GetGrantedPermission(pluginName, groupName, permissionName) != null)
            {
                return true;
            }

            PermissionGroup group = null;

            if (String.IsNullOrEmpty(pluginName))
            {
                group = CmsConfiguration.Instance.Security.PermissionGroups.FirstOrDefault(it => it.Name == groupName);
            }
            else
            {
                var plugin = PluginManager.FindEnabledPlugin(pluginName);
                if (plugin != null)
                {
                    group = plugin.PermissionGroups.Find(groupName);
                }
            }

            if (group == null) return false;

            var permission = group.Permissions.Find(permissionName);
            var higherWeighted = group.Permissions.Where(it => it.Weight > permission.Weight);

            foreach (var each in higherWeighted)
            {
                if (GetGrantedPermission(each) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void AddPermission(Permission permission)
        {
            var pluginName = permission.Plugin == null ? null : permission.Plugin.Name;
            var groupName = permission.Group == null ? null : permission.Group.Name;

            if (!HasPermission(pluginName, groupName, permission.Name))
            {
                GrantedPermissions.Add(new RoleGrantedPermission(this)
                {
                    PermissionGroupName = groupName,
                    PermissionName = permission.Name,
                    PluginName = pluginName
                });
            }
        }

        public virtual bool RemovePermission(Permission permission)
        {
            var pluginName = permission.Plugin == null ? null : permission.Plugin.Name;
            var groupName = permission.Group == null ? null : permission.Group.Name;

            return RemovePermission(pluginName, groupName, permission.Name);
        }

        public virtual bool RemovePermission(string pluginName, string groupName, string permissionName)
        {
            var permission = GetGrantedPermission(pluginName, groupName, permissionName);
            if (permission != null)
            {
                return GrantedPermissions.Remove(permission);
            }

            return false;
        }

        public virtual void RemovePermissionsByGroup(string pluginName, string groupName)
        {
            IEnumerable<RoleGrantedPermission> permissions = GrantedPermissions;

            if (String.IsNullOrEmpty(pluginName))
            {
                permissions = permissions.Where(it => String.IsNullOrEmpty(it.PluginName));
            }
            else
            {
                permissions = permissions.Where(it => it.PluginName == pluginName);
            }

            permissions = permissions.Where(it => it.PermissionGroupName == groupName);

            foreach (var each in permissions.ToList())
            {
                GrantedPermissions.Remove(each);
            }
        }

        private RoleGrantedPermission GetGrantedPermission(Permission permission)
        {
            var pluginName = permission.Plugin == null ? null : permission.Plugin.Name;
            var groupName = permission.Group == null ? null : permission.Group.Name;

            return GetGrantedPermission(pluginName, groupName, permission.Name);
        }

        private RoleGrantedPermission GetGrantedPermission(string pluginName, string groupName, string permissionName)
        {
            IEnumerable<RoleGrantedPermission> permissions = GrantedPermissions;

            if (String.IsNullOrEmpty(pluginName))
            {
                permissions = permissions.Where(it => String.IsNullOrEmpty(pluginName));
            }
            else
            {
                permissions = permissions.Where(it => it.PluginName == pluginName);
            }

            if (String.IsNullOrEmpty(groupName))
            {
                permissions = permissions.Where(it => String.IsNullOrEmpty(groupName));
            }
            else
            {
                permissions = permissions.Where(it => it.PermissionGroupName == groupName);
            }

            return permissions.FirstOrDefault(it => it.PermissionName == permissionName);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
