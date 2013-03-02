using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.ImageSlider.Domain
{
    public class Slider
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<SliderItem> Items { get; protected set; }

        public virtual DateTime UtcCreatedAt { get; protected set; }

        public Slider()
        {
            Items = new List<SliderItem>();
            UtcCreatedAt = DateTime.UtcNow;
        }
    }
}