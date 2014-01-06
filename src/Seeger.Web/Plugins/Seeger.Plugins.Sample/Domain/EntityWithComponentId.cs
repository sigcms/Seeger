using NHibernate.Mapping.ByCode.Conformist;
using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Sample.Domain
{
    [Entity]
    public class EntityWithComponentId
    {
        public virtual Identity Id { get; set; }

        public virtual string Property1 { get; set; }
    }

    //public class EntityWithComponentIdMap : ClassMapping<EntityWithComponentId>
    //{
    //    public EntityWithComponentIdMap()
    //    {
    //        ComponentAsId(c => c.Id, m =>
    //        {
    //            m.Property(x => x.Field1);
    //            m.Property(x => x.Field2);
    //        });
    //    }
    //}

    public class Identity
    {
        public int Field1 { get; set; }

        public int Field2 { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Identity;
            return other != null && other.Field1 == Field1 && other.Field2 == Field2;
        }

        public override int GetHashCode()
        {
            return Field1.GetHashCode() ^ Field2.GetHashCode();
        }
    }
}