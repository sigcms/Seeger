using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    public interface IAttributeMapperFactory
    {
        IEnumerable<IIdAttributeMapper> GetIdAttributeMappers(Attribute attribute);

        IEnumerable<IClassAttributeMapper> GetClassAttributeMappers(Attribute attribute);

        IEnumerable<IPropertyAttributeMapper> GetPropertyAttributeMappers(Attribute attribute);
    }

    public static class AttributeMapperFactories
    {
        public static IAttributeMapperFactory Current;
    }

    public class AttributeMapperFactory : IAttributeMapperFactory
    {
        readonly ConcurrentDictionary<Type, ConcurrentBag<object>> _mappersByAttrType = new ConcurrentDictionary<Type, ConcurrentBag<object>>();

        public void RegisterMappers(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                RegisterMappers(assembly.GetTypes());
            }
        }

        public void RegisterMappers(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var mapperForAttr = type.GetCustomAttributes(typeof(MapperForAttribute), false)
                                        .OfType<MapperForAttribute>()
                                        .FirstOrDefault();

                if (mapperForAttr != null)
                {
                    var mappers = _mappersByAttrType.GetOrAdd(mapperForAttr.AttributeType, new ConcurrentBag<object>());
                    mappers.Add(Activator.CreateInstance(type));
                }
            }
        }

        public IEnumerable<IIdAttributeMapper> GetIdAttributeMappers(Attribute attribute)
        {
            return GetMappers(attribute).OfType<IIdAttributeMapper>();
        }

        public IEnumerable<IClassAttributeMapper> GetClassAttributeMappers(Attribute attribute)
        {
            return GetMappers(attribute).OfType<IClassAttributeMapper>();
        }

        public IEnumerable<IPropertyAttributeMapper> GetPropertyAttributeMappers(Attribute attribute)
        {
            return GetMappers(attribute).OfType<IPropertyAttributeMapper>();
        }

        private IEnumerable<object> GetMappers(Attribute attribute)
        {
            ConcurrentBag<object> mappers;

            if (_mappersByAttrType.TryGetValue(attribute.GetType(), out mappers))
            {
                return mappers.ToList();
            }

            return Enumerable.Empty<object>();
        }
    }
}
