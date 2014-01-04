using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Security;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping.Impl
{
    class RoleGrantedPermissionMap : ClassMapping<RoleGrantedPermission>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(RoleGrantedPermission).Name;
            }
        }

        public RoleGrantedPermissionMap()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.PluginName);
            Property(c => c.PermissionGroupName);
            Property(c => c.PermissionName);

            ManyToOne(it => it.Role, m => m.Column("RoleId"));
        }
    }
}
