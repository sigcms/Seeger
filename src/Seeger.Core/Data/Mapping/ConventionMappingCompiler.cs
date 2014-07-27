using NHibernate;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using Seeger.ComponentModel;
using Seeger.Data.Mapping.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Data.Mapping
{
    public class ConventionMappingCompiler
    {
        private ConventionModelMapper _mapper;
        private HashSet<Type> _entityTypes = new HashSet<Type>();

        public ConventionModelMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        public ConventionMappingCompiler()
            : this(null, AttributeMapperFactories.Current)
        {
        }

        public ConventionMappingCompiler(IAttributeMapperFactory attributeMapperFactory)
            : this(null, attributeMapperFactory)
        {
        }

        public ConventionMappingCompiler(string tablePrefix)
            : this(tablePrefix, AttributeMapperFactories.Current)
        {
        }

        public ConventionMappingCompiler(string tablePrefix, IAttributeMapperFactory attributeMapperFactory)
        {
            _mapper = new ConventionModelMapper(tablePrefix)
            {
                AttributeMapperFactory = attributeMapperFactory
            };
        }

        public ConventionMappingCompiler AddMappings(IEnumerable<Type> types)
        {
            _mapper.AddMappings(types);
            return this;
        }

        public ConventionMappingCompiler AddAssemblies(params Assembly[] assemblies)
        {
            return AddAssemblies(assemblies as IEnumerable<Assembly>);
        }

        public ConventionMappingCompiler AddAssemblies(IEnumerable<Assembly> assemblies)
        {
            Require.NotNull(assemblies, "assemblies");

            foreach (var asm in assemblies)
            {
                var types = asm.GetTypes();
                AddMappings(types);
                AddEntityTypes(types.Where(t => IsEntity(t)));
            }

            return this;
        }

        public ConventionMappingCompiler AddEntityTypes(params Type[] types)
        {
            return AddEntityTypes(types as IEnumerable<Type>);
        }

        public ConventionMappingCompiler AddEntityTypes(IEnumerable<Type> types)
        {
            Require.NotNull(types, "types");

            foreach (var type in types)
            {
                _entityTypes.Add(type);
            }

            return this;
        }

        private bool IsEntity(Type type)
        {
            return type.GetCustomAttributes(typeof(EntityAttribute), false).Any() && _mapper.ModelInspector.IsEntity(type);
        }

        public HbmMapping CompileMapping()
        {
            var types = new HashSet<Type>(_entityTypes);

            foreach (var type in _mapper.CustomizersHolder.GetAllCustomizedEntities())
            {
                types.Add(type);
            }

            var mapping = _mapper.CompileMappingFor(types);
            mapping.autoimport = false;

            return mapping;
        }
    }
}
