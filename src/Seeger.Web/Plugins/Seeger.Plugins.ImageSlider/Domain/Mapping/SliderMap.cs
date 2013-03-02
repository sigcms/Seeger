using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seeger.Data.Mapping;
using NHibernate.Mapping.ByCode;

namespace Seeger.Plugins.ImageSlider.Domain.Mapping
{
    public class SliderMap : ClassMapping<Slider>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(Slider).Name;
            }
        }

        public SliderMap()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.Name);
            Property(c => c.UtcCreatedAt);

            Bag(c => c.Items, m =>
            {
                m.Key(k =>
                {
                    k.Column("SliderId");
                    k.NotNullable(true);
                });
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
            }, m => m.OneToMany());
        }
    }
}