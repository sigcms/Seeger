using Seeger.Data.Mapping;
using Seeger.Data.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seeger.Security
{
    [Entity]
    public class RoleGrantedPermission
    {
        [HiloId]
        public virtual int Id { get; set; }

        public virtual string PluginName { get; set; }

        public virtual string PermissionGroupName { get; set; }

        public virtual string PermissionName { get; set; }
        
        public virtual Role Role { get; set; }

        public RoleGrantedPermission()
        {
        }

        public RoleGrantedPermission(Role role)
        {
            Role = role;
        }

        public override string ToString()
        {
            return String.IsNullOrEmpty(PermissionGroupName) ? PermissionName : PermissionGroupName + " " + PermissionName;
        }
    }
}
