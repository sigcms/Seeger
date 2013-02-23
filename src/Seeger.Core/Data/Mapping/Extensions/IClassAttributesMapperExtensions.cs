using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NHibernate.Mapping.ByCode;

namespace Seeger.Data.Mapping
{
    public static class IClassAttributesMapperExtensions
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
            mapper.Id(idProperty, m =>
            {
                MapHiLoId(m, entityTableName, hiValueTableName, nextValueColumnName, maxLowValue);
            });
        }

        public static void HighLowId<TEntity>(this IClassAttributesMapper<TEntity> mapper,
            string idProperty,
            string entityTableName,
            string hiValueTableName,
            string nextValueColumnName,
            int maxLowValue)
            where TEntity : class
        {
            mapper.Id(idProperty, m =>
            {
                MapHiLoId(m, entityTableName, hiValueTableName, nextValueColumnName, maxLowValue);
            });
        }

        private static void MapHiLoId(
            IIdMapper mapper,
            string entityTableName,
            string hiValueTableName,
            string nextValueColumnName,
            int maxLowValue)
        {
            mapper.Generator(Generators.HighLow, g =>
            {
                g.Params(new
                {
                    table = hiValueTableName,
                    column = nextValueColumnName,
                    max_lo = maxLowValue,
                    where = "TableName = '" + entityTableName + "'"
                });
            });
        }
    }
}
