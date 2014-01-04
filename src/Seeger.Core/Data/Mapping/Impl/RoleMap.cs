using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Security;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping.Impl
{
    class RoleMap : ClassMapping<Role>
    {
        public RoleMap()
        {
            Bag<RoleGrantedPermission>(c => c.GrantedPermissions, m =>
            {
                m.Key(k => k.Column("RoleId"));

                m.Inverse(true);
                m.Lazy(CollectionLazy.NoLazy);
                m.Fetch(CollectionFetchMode.Join);
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
            }, m => m.OneToMany());
        }
    }
}
