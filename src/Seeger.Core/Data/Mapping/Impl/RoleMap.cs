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
        public string TableName
        {
            get
            {
                return "cms_" + typeof(Role).Name;
            }
        }

        public RoleMap()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.Name);
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
