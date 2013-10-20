using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NHibernate.Mapping.ByCode;

namespace Seeger.Data.Mapping
{
    public static class IdMapperExtensions
    {
        public static void HighLowId<TEntity, TProperty>(this IClassAttributesMapper<TEntity> mapper,
            Expression<Func<TEntity, TProperty>> idProperty,
            string entityTableName)
            where TEntity : class
        {
            HighLowId<TEntity, TProperty>(mapper, idProperty, entityTableName, "cms_HiValue", "NextValue", 10);
        }

        public static void HighLowId<TEntity>(this IClassAttributesMapper<TEntity> mapper,
            string idProperty,
            string entityTableName)
            where TEntity : class
        {
            HighLowId<TEntity>(mapper, idProperty, entityTableName, "cms_HiValue", "NextValue", 10);
        }

        public static void HighLowId<TEntity, TProperty>(this IClassAttributesMapper<TEntity> mapper,
            Expression<Func<TEntity, TProperty>> idProperty,
            string entityTableName,
            string hiValueTableName,
            string nextValueColumnName,
            int maxLowValue)
            where TEntity : class
        {
            mapper.Id(idProperty, IdMappings.HighLowId(entityTableName, hiValueTableName, nextValueColumnName, maxLowValue));
        }

        public static void HighLowId<TEntity>(this IClassAttributesMapper<TEntity> mapper,
            string idProperty,
            string entityTableName,
            string hiValueTableName,
            string nextValueColumnName,
            int maxLowValue)
            where TEntity : class
        {
            mapper.Id(idProperty, IdMappings.HighLowId(entityTableName, hiValueTableName, nextValueColumnName, maxLowValue));
        }

        public static void HighLowId<TEntity, TElement>(this IIdBagPropertiesMapper<TEntity, TElement> mapper, string table)
            where TEntity : class
        {
            mapper.HighLowId(table, "cms_HiValue", "NextValue", 10);
        }

        public static void HighLowId<TEntity, TElement>(this IIdBagPropertiesMapper<TEntity, TElement> mapper, string table, string hiValueTable, string nextValueColumn, int maxLow)
            where TEntity : class
        {
            mapper.Table(table);
            mapper.Id(IdMappings.CollectionHighLowId(table, hiValueTable, nextValueColumn, maxLow));
        }
    }
}
