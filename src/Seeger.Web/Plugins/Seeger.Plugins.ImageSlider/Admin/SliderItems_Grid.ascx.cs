using Seeger.Web.UI.Grid;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Web.UI;

namespace Seeger.Plugins.ImageSlider.Admin
{
    public partial class SliderItems_Grid : AjaxGridUserControlBase
    {
        protected int SliderId { get; private set; }

        protected bool CanEdit
        {
            get
            {
                return AdminSession.Current.User.HasPermission(Strings.PluginName, "ImageSlider", "EditSliderItem");
            }
        }

        protected bool CanDelete
        {
            get
            {
                return AdminSession.Current.User.HasPermission(Strings.PluginName, "ImageSlider", "DeleteSliderItem");
            }
        }

        public override void Bind(AjaxGridContext context)
        {
            SliderId = Convert.ToInt32(context.QueryString["sliderId"]);
            
            var slider = NhSession.Get<Slider>(SliderId);

            List.DataSource = slider.Items.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id);
            List.DataBind();
        }
    }
}