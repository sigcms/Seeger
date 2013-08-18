using NHibernate.Mapping.ByCode.Conformist;
using Seeger.Plugins.Sample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seeger.Data.Mapping;
using NHibernate;

namespace Seeger.Plugins.Sample.Data.Mapping
{
    public class BlogPostMap : ClassMapping<BlogPost>
    {
        public string TableName
        {
            get
            {
                return "sample_" + typeof(BlogPost).Name;
            }
        }

        public BlogPostMap()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.Title);
            Property(c => c.Content, m => m.Type(NHibernateUtil.StringClob));
            Property(c => c.CreatedAt);
        }
    }
}