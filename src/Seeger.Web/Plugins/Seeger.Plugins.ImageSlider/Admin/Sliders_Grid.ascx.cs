using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Web.UI;

namespace Seeger.Plugins.ImageSlider.Admin
{
    public partial class Sliders_Grid : AjaxGridUserControlBase
    {
        protected bool CanDelete
        {
            get
            {
                return AdminSession.Current.User.HasPermission(Strings.PluginName, "ImageSlider", "DeleteSlider");
            }
        }

        public override void Bind(AjaxGridContext context)
        {
            var sliders = NhSession.Query<Slider>()
                                   .OrderBy(x => x.UtcCreatedAt)
                                   .Paging(Pager.PageSize);

            List.DataSource = sliders.Page(context.PageIndex);
            List.DataBind();

            Pager.RecordCount = sliders.Count;
            Pager.PageIndex = context.PageIndex;
        }
    }
}