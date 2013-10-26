using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Seeger.Globalization;
using Seeger.Plugins.RichText.Domain;

namespace Seeger.Plugins.RichText.Widgets.RichText
{
    public partial class Designer : Seeger.Web.UI.WidgetControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDesigner();
            }
        }

        private void BindDesigner()
        {
            if (WidgetAttributes != null)
            {
                if (WidgetAttributes.ContainsKey("ContentId"))
                {
                    int contentId = WidgetAttributes.GetValue<int>("ContentId");
                    var textContent = NhSession.Get<TextContent>(contentId);
                    if (textContent != null)
                    {
                        var text = textContent.Name;

                        if (FrontendSettings.Multilingual)
                        {
                            text = textContent.GetLocalized(x => x.Name, PageCulture);
                        }

                        if (String.IsNullOrEmpty(text))
                        {
                            text = textContent.Content;

                            if (FrontendSettings.Multilingual)
                            {
                                text = textContent.GetLocalized(x => x.Content, PageCulture);
                            }
                        }

                        if (String.IsNullOrEmpty(text))
                        {
                            ContentText.Text = "<div style='text-align:center;padding-top:10px;padding-bottom:10px'>[" + ResourceFolder.Global.GetValue("Common.Empty", CultureInfo.CurrentUICulture) + "]</div>";
                        }
                        else
                        {
                            ContentText.Text = text;
                        }
                    }
                }
            }
        }
    }
}