using Seeger.Data;
using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Web.UI;
using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.ImageSlider.Admin
{
    public partial class SliderItems : AjaxGridPageBase
    {
        protected int SliderId { get; private set; }

        protected bool CanAdd
        {
            get
            {
                return AdminSession.User.HasPermission(Strings.PluginName, "ImageSlider", "AddSliderItem");
            }
        }

        public override bool VerifyAccess(Security.User user)
        {
            return user.HasPermission(Strings.PluginName, "ImageSlider", "ViewSliders");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SliderId = Convert.ToInt32(Request.QueryString["sliderId"]);
        }

        [WebMethod, ScriptMethod]
        public static void Delete(int sliderId, int itemId)
        {
            if (!AdminSession.Current.User.HasPermission(Strings.PluginName, "ImageSlider", "DeleteSliderItem"))
                throw new InvalidOperationException("Access denied.");

            var db = Database.GetCurrentSession();
            var slider = db.Get<Slider>(sliderId);
            var item = slider.Items.FirstOrDefault(x => x.Id == itemId);
            slider.Items.Remove(item);
            db.Commit();
        }
    }
}