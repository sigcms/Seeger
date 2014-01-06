using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping
{
    public class MappingConventions
    {
        public string TablePrefix { get; set; }

        public string GetTableName(Type type)
        {
            if (String.IsNullOrEmpty(TablePrefix))
            {
                return type.Name;
            }

            var prefix = TablePrefix;
            if (!prefix.EndsWith("_"))
            {
                prefix = prefix + "_";
            }

            return prefix + type.Name;
        }
    }
}
