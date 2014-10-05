using Seeger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments.Data
{
    public class CommentNhMappingProvider : INhMappingProvider
    {
        public IEnumerable<NHibernate.Cfg.MappingSchema.HbmMapping> GetMappings()
        {
            var compiler = new Seeger.Data.Mapping.ConventionMappingCompiler("cmt");
            yield return compiler.AddAssemblies(typeof(CommentNhMappingProvider).Assembly).CompileMapping();
        }
    }
}