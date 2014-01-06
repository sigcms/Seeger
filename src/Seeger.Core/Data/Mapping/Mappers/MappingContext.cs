using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    public class MappingContext
    {
        public MappingConventions Conventions { get; private set; }

        public IModelInspector ModelInspector { get; private set; }

        public MappingContext(IModelInspector modelInspector, MappingConventions conventions)
        {
            ModelInspector = modelInspector;
            Conventions = conventions;
        }
    }
}
