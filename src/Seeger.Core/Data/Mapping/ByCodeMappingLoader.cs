using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping
{
    public static class ByCodeMappingLoader
    {
        public static HbmMapping LoadMappingFrom(params Assembly[] assemblies)
        {
            Require.NotNull(assemblies, "assemblies");

            var baseTypes = new Type[] {
                typeof(ClassMapping<>),
                typeof(JoinedSubclassMapping<>),
                typeof(UnionSubclassMapping<>),
                typeof(SubclassMapping<>)
            };

            var mapper = new ModelMapper();
            
            foreach (var asm in assemblies)
            {
                foreach (var type in asm.GetTypes())
                {
                    if (!type.IsClass || type.IsAbstract || type.IsInterface || !type.BaseType.IsGenericType) continue;

                    if (!baseTypes.Contains(type.BaseType.GetGenericTypeDefinition())) continue;

                    mapper.AddMapping(type);
                }
            }

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            mapping.autoimport = false;

            return mapping;
        }
    }
}
