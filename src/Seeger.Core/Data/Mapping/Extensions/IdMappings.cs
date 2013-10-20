using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping
{
    static class IdMappings
    {
        public static Action<IIdMapper> HighLowId(string entityTable)
        {
            return HighLowId(entityTable, "cms_HiValue", "NextValue", 10);
        }

        public static Action<IIdMapper> HighLowId(string entityTable, string hiValueTable, string nextValueColumn, int maxLow)
        {
            return mapper =>
            {
                mapper.Generator(Generators.HighLow, g =>
                {
                    g.Params(HighLowParams(entityTable, hiValueTable, nextValueColumn, maxLow));
                });
            };
        }

        public static Action<ICollectionIdMapper> CollectionHighLowId(string entityTable, string hiValueTable, string nextValueColumn, int maxLow)
        {
            return mapper =>
            {
                mapper.Generator(Generators.HighLow, g =>
                {
                    g.Params(HighLowParams(entityTable, hiValueTable, nextValueColumn, maxLow));
                });
            };
        }

        static object HighLowParams(string entityTable, string hiValueTable, string nextValueColumn, int maxLow)
        {
            return new
            {
                table = hiValueTable,
                column = nextValueColumn,
                max_lo = maxLow,
                where = "TableName='" + entityTable + "'"
            };
        }
    }
}
