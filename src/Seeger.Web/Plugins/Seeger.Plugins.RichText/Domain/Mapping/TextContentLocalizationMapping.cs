using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using Seeger.Data.Mapping;

namespace Seeger.Plugins.RichText.Domain.Mapping
{
    class TextContentLocalizationMapping : ClassMapping<TextContentLocalization>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(TextContentLocalization).Name;
            }
        }

        public TextContentLocalizationMapping()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.Culture);
            Property(c => c.Content);
            ManyToOne<TextContent>("Owner", m => m.Column("ContentId"));
        }
    }
}
