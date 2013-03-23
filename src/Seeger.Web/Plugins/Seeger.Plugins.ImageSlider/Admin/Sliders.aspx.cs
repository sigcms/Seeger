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
    public partial class Sliders : AjaxGridPageBase
    {
        public override bool VerifyAccess(Security.User user)
        {
            return user.HasPermission(Strings.PluginName, "ImageSlider", "ViewSliders");
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod, ScriptMethod]
        public static void Delete(int id)
        {
            if (!AdminSession.Current.User.HasPermission(Strings.PluginName, "ImageSlider", "DeleteSlider"))
                throw new InvalidOperationException("Access denied.");

            var db = Database.GetCurrentSession();
            var slider = db.Get<Slider>(id);
            db.Delete(slider);
            db.Commit();
        }
    }
}