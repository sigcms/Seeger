using Seeger.Data;
using Seeger.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Sample.Data
{
    public class NhMappingProvider : INhMappingProvider
    {
        public IEnumerable<NHibernate.Cfg.MappingSchema.HbmMapping> GetMappings()
        {
            yield return ByCodeMappingLoader.LoadMappingFrom(typeof(NhMappingProvider).Assembly);
        }
    }
}