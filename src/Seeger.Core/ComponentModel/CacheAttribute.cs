using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CacheAttribute : Attribute
    {
        public CacheUsage Usage { get; set; }

        public CacheInclude Include { get; set; }

        public string Region { get; set; }

        public CacheAttribute()
        {
            Usage = CacheUsage.ReadWrite;
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
