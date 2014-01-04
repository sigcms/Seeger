using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping.Impl
{
    class GlobalSettingMap : ClassMapping<GlobalSetting>
    {
        public GlobalSettingMap()
        {
            Table("cms_" + typeof(GlobalSetting).Name);

            Id(c => c.Key, m =>
            {
                m.Column("`Key`");
                m.Generator(Generators.Assigned);
            });
            Property(c => c.Value);
        }
    }
}
