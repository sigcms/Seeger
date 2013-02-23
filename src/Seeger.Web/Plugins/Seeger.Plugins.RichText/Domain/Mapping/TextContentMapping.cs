using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate;
using Seeger.Data.Mapping;

namespace Seeger.Plugins.RichText.Domain.Mapping
{
    class TextContentMapping : ClassMapping<TextContent>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(TextContent).Name;
            }
        }

        public TextContentMapping()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.Name);
            Property(c => c.CreatedTime);
            Property(c => c.Content, m => m.Type(NHibernateUtil.StringClob));
        }
    }
}
