using Seeger.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Seeger.Plugins.RichText
{
    public class NhMappingProvider : Seeger.Data.INhMappingProvider
    {
        public IEnumerable<NHibernate.Cfg.MappingSchema.HbmMapping> GetMappings()
        {
            yield return new ConventionMappingCompiler("cms").AddAssemblies(typeof(NhMappingProvider).Assembly).CompileMapping();
        }
    }
}