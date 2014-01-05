using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CacheAttribute : ClassLevelAttribute
    {
        public CacheUsage Usage { get; set; }

        public CacheInclude Include { get; set; }

        public string Region { get; set; }

        public CacheAttribute()
        {
            Usage = CacheUsage.ReadWrite;
        }

        public override void ApplyMapping(IModelInspector modelInspector, Type type, IClassAttributesMapper mapper)
        {
            mapper.Cache(m =>
            {
                m.Usage(GetNHibernateCacheUsage());
                m.Include(GetNHibernateCacheInclude());

                if (!String.IsNullOrEmpty(Region))
                {
                    m.Region(Region);
                }
            });
        }

        private NHibernate.Mapping.ByCode.CacheUsage GetNHibernateCacheUsage()
        {
            switch (Usage)
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
                    throw new NotSupportedException("Not support cache usage type: " + Usage + ".");
            }
        }

        private NHibernate.Mapping.ByCode.CacheInclude GetNHibernateCacheInclude()
        {
            switch (Include)
            {
                case CacheInclude.All:
                    return NHibernate.Mapping.ByCode.CacheInclude.All;
                case CacheInclude.NonLazy:
                    return NHibernate.Mapping.ByCode.CacheInclude.NonLazy;
                default:
                    throw new NotSupportedException("Not support cache include type: " + Include + ".");
            }
        }
    }

    public enum CacheUsage
    {
        NonstrictReadWrite = 0,
        ReadOnly = 1,
        ReadWrite = 2,
        Transactional = 3
    }

    public enum CacheInclude
    {
        All = 0,
        NonLazy = 1
    }
}
