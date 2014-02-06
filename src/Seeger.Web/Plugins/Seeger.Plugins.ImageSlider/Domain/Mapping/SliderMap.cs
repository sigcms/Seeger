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
        public SliderMap()
        {
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