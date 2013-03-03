using Seeger.Plugins.ImageSlider.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.ImageSlider.Widgets.ImageSlider
{
    public partial class Default : Seeger.Web.UI.WidgetControlBase
    {
        protected Slider Slider { get; private set; }

        protected IList<SliderItem> Items { get; private set; }

        public Default()
        {
            Items = new List<SliderItem>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var sliderId = WidgetAttributes.GetValue<int>("SliderId");

            if (sliderId > 0)
            {
                Slider = NhSession.Get<Slider>(sliderId);
                Items = Slider.Items;
            }
            else
            {
                Visible = false;
            }
        }
    }
}