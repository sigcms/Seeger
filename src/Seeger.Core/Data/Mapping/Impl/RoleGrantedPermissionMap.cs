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
        public RoleGrantedPermissionMap()
        {
            ManyToOne(it => it.Role, m => m.Column("RoleId"));
        }
    }
}
