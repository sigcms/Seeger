using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

using Seeger.Security;

namespace Seeger.Data.Mapping.Impl
{
    class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Property("_skinName", m =>
            {
                m.Column("SkinName");
                m.Access(Accessor.Field);
            });

            IdBag<Role>(c => c.Roles, m =>
            {
                m.Table("cms_UserInRole");
                m.Key(x => x.Column("UserId"));
                m.Id(id =>
                {
                    id.Column("Id");
                    id.Generator(Generators.HighLow, g =>
                    {
                        g.Params(new
                        {
                            table = "cms_HiValue",
                            column = "NextValue",
                            max_lo = 10,
                            where = "TableName = 'cms_UserInRole'"
                        });
                    });
                });
            },
            m => m.ManyToMany(mm => mm.Column("RoleId")));
        }
    }
}
