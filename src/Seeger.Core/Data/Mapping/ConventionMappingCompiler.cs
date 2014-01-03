using NHibernate;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using Seeger.ComponentModel;
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

        public ConventionMappingCompiler()
            : this(null)
        {
        }

        public ConventionMappingCompiler(string tablePrefix)
        {
            _mapper = new ConventionModelMapper(tablePrefix);
        }

        public ConventionMappingCompiler AddMappings(IEnumerable<Type> types)
        {
            Require.NotNull(types, "types");

            foreach (var type in types.Where(x => typeof(IConformistHoldersProvider).IsAssignableFrom(x) && !x.IsGenericTypeDefinition))
            {
                AddMapping(type);
            }

            return this;
        }

        private void AddMapping(Type type)
        {
            object mappingInstance;
            try
            {
                mappingInstance = Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                throw new MappingException("Unable to instantiate mapping class (see InnerException): " + type, e);
            }

            var mapping = mappingInstance as IConformistHoldersProvider;
            if (mapping == null)
            {
                throw new ArgumentOutOfRangeException("type", "The mapping class must be an implementation of IConformistHoldersProvider.");
            }

            AddMapping(mapping);
        }

        private void AddMapping(IConformistHoldersProvider mapping)
        {
            _mapper.AddMapping(mapping);

            foreach (var type in mapping.CustomizersHolder.GetAllCustomizedEntities())
            {
                _entityTypes.Add(type);
            }
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
                AddEntityTypes(types);
            }

            return this;
        }

        public ConventionMappingCompiler AddEntityTypes(IEnumerable<Type> types)
        {
            Require.NotNull(types, "types");

            foreach (var type in types.Where(t => IsEntityType(t)))
            {
                _entityTypes.Add(type);
            }

            return this;
        }

        private bool IsEntityType(Type type)
        {
            return type.GetCustomAttributes(typeof(EntityAttribute), false).Any();
        }

        public HbmMapping CompileMapping()
        {
            var mapping = _mapper.CompileMappingFor(_entityTypes);
            mapping.autoimport = false;

            return mapping;
        }
    }
}
