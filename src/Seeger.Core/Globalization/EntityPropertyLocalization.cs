using Seeger.Data.Mapping;
using Seeger.Data.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Globalization
{
    [Entity]
    public class EntityPropertyLocalization
    {
        [HiloId]
        public virtual int Id { get; set; }

        public virtual string EntityType { get; set; }

        public virtual string EntityKey { get; set; }

        public virtual string PropertyPath { get; set; }

        public virtual string Culture { get; set; }

        [StringClob]
        public virtual string PropertyValue { get; set; }
    }
}
