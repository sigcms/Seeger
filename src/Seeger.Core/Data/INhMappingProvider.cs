using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data
{
    public interface INhMappingProvider
    {
        IEnumerable<HbmMapping> GetMappings();
    }
}
