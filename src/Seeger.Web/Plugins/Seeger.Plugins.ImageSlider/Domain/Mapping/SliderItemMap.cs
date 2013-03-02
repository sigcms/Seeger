using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seeger.Data.Mapping;

namespace Seeger.Plugins.ImageSlider.Domain.Mapping
{
    public class SliderItemMap : ClassMapping<SliderItem>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(SliderItem).Name;
            }
        }

        public SliderItemMap()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.Caption);
            Property(c => c.Description);
            Property(c => c.ImageUrl);
            Property(c => c.NavigateUrl);
            Property(c => c.DisplayOrder);
        }
    }
}