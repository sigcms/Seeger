using Seeger.ComponentModel;
using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.ImageSlider.Domain
{
    [Entity]
    public class Slider
    {
        public virtual int Id { get; set; }

        public virtual int? Width { get; set; }

        public virtual int? Height { get; set; }

        public virtual bool ShowNavigation { get; set; }

        public virtual bool ShowPagination { get; set; }

        public virtual bool AutoPlay { get; set; }

        public virtual int AutoPlayInterval { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<SliderItem> Items { get; protected set; }

        public virtual DateTime UtcCreatedAt { get; protected set; }

        public Slider()
        {
            Items = new List<SliderItem>();
            UtcCreatedAt = DateTime.UtcNow;
            ShowPagination = true;
            ShowNavigation = true;
            AutoPlay = true;
            AutoPlayInterval = 5000;
        }
    }
}