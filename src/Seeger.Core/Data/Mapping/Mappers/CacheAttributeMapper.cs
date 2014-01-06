using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    [MapperFor(typeof(CacheAttribute))]
    public class CacheAttributeMapper : IClassAttributeMapper
    {
        public void ApplyMapping(Attribute attribute, Type type, NHibernate.Mapping.ByCode.IClassAttributesMapper mapper, MappingContext context)
        {
            var cacheAttr = (CacheAttribute)attribute;

            mapper.Cache(m =>
            {
                m.Usage(GetNHibernateCacheUsage(cacheAttr));
                m.Include(GetNHibernateCacheInclude(cacheAttr));

                if (!String.IsNullOrEmpty(cacheAttr.Region))
                {
                    m.Region(cacheAttr.Region);
                }
            });
        }

        private NHibernate.Mapping.ByCode.CacheUsage GetNHibernateCacheUsage(CacheAttribute attribute)
        {
            switch (attribute.Usage)
            {
                case CacheUsage.NonstrictReadWrite:
                    return NHibernate.Mapping.ByCode.CacheUsage.NonstrictReadWrite;
                case CacheUsage.ReadOnly:
                    return NHibernate.Mapping.ByCode.CacheUsage.ReadOnly;
                case CacheUsage.ReadWrite:
                    return NHibernate.Mapping.ByCode.CacheUsage.ReadWrite;
                case CacheUsage.Transactional:
                    return NHibernate.Mapping.ByCode.CacheUsage.Transactional;
                default:
                    throw new NotSupportedException("Not support cache usage type: " + attribute.Usage + ".");
            }
        }

        private NHibernate.Mapping.ByCode.CacheInclude GetNHibernateCacheInclude(CacheAttribute attribute)
        {
            switch (attribute.Include)
            {
                case CacheInclude.All:
                    return NHibernate.Mapping.ByCode.CacheInclude.All;
                case CacheInclude.NonLazy:
                    return NHibernate.Mapping.ByCode.CacheInclude.NonLazy;
                default:
                    throw new NotSupportedException("Not support cache include type: " + attribute.Include + ".");
            }
        }
    }
}
