using Newtonsoft.Json;
using Seeger.Data;
using Seeger.Globalization;
using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Plugins.ImageSlider.Models;
using Seeger.Utils;
using Seeger.Web;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        protected int SliderId { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            SliderId = WidgetAttributes.GetValue<int>("SliderId");
        }

        [WebMethod, ScriptMethod]
        public static string GetViewModel(int sliderId)
        {
            var session = Database.GetCurrentSession();
            var model = new SliderWidgetEditorViewModel();

            if (sliderId > 0)
            {
                model.Slider = session.Get<Slider>(sliderId);
            }
            else
            {
                model.Slider = new Slider
                {
                    Name = ResourceFolder.Global.GetValue("Common.Unnamed", CultureInfo.CurrentUICulture)
                };
            }

            return JsonConvertUtil.CamelCaseSerializeObject(model);
        }
    }
}