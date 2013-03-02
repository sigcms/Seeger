using Newtonsoft.Json;
using Seeger.Data;
using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Utils;
using Seeger.Web;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.ImageSlider.Widgets.ImageSlider
{
    public partial class Editor : WidgetEditorBase
    {
        protected string AspNetAuth
        {
            get
            {
                return Request.GetAuthCookieValue();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod, ScriptMethod]
        public static string GetViewModel(int sliderId)
        {
            var session = NhSessionManager.GetCurrentSession();
            var model = new ViewModel();

            if (sliderId > 0)
            {
                model.Slider = session.Get<Slider>(sliderId);
            }
            else
            {
                model.Slider = new Slider();
            }

            return JsonConvertUtil.CamelCaseSerializeObject(model);
        }
    }

    public class ViewModel
    {
        public Slider Slider { get; set; }
    }
}