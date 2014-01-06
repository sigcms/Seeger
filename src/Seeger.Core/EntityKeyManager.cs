using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using Seeger.ComponentModel;
using Seeger.Data.Mapping;

namespace Seeger
{
    public static class EntityKeyManager
    {
        static Dictionary<Type, PropertyInfo> _cache = new Dictionary<Type, PropertyInfo>();

        public static string GetEntityKeyPropertyName<TEntity>()
        {
            return GetEntityKeyPropertyName(typeof(TEntity));
        }

        public static string GetEntityKeyPropertyName(Type entityType)
        {
            var prop = GetEntityKeyProperty(entityType);
            return prop == null ? null : prop.Name;
        }

        public static Type GetEntityKeyType(Type entityType)
        {
            var prop = GetEntityKeyProperty(entityType);
            return prop == null ? null : prop.PropertyType;
        }

        public static PropertyInfo GetEntityKeyProperty(Type entityType)
        {
            PropertyInfo prop = null;

            if (!_cache.TryGetValue(entityType, out prop))
            {
                lock (_cache)
                {
                    if (!_cache.TryGetValue(entityType, out prop))
                    {
                        prop = DoGetEntityKeyProperty(entityType);
                        _cache.Add(entityType, prop);
                    }
                }
            }

            return prop;
        }

        public static object GetEntityKey(object entity)
        {
            Require.NotNull(entity, "entity");

            var entityType = entity.GetType();
            var propName = GetEntityKeyPropertyName(entityType);

            if (propName == null)
                throw new InvalidOperationException("Cannot find entity key. Ensure the entity key property is attributed with " + typeof(IdAttribute) + ". Entity type: " + entityType);

            var prop = entityType.GetProperty(propName);

            if (prop == null)
                throw new InvalidOperationException("Cannot load PropertyInfo for property \"" + propName + "\" of type " + entityType);

            return prop.GetValue(entity, null);
        }

        static PropertyInfo DoGetEntityKeyProperty(Type entityType)
        {
            foreach (var prop in entityType.GetProperties())
            {
                var attr = prop.GetCustomAttributes(typeof(IdAttribute), false).FirstOrDefault() as IdAttribute;
                if (attr != null) return prop;
            }

            return entityType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
