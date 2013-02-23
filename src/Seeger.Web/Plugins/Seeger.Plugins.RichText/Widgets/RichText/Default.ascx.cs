using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Data;
using Seeger.Web.UI;
using Seeger.Plugins.RichText.Domain;

namespace Seeger.Plugins.RichText.Widgets.RichText
{
    public partial class Default : WidgetControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindContent();
            }
        }

        private void BindContent()
        {
            int contentId = WidgetAttributes.GetValue<int>("ContentId", 0);
            if (contentId > 0)
            {
                var content = NhSession.Get<TextContent>(contentId);
                if (content != null)
                {
                    ContentBody.Text = content.Content;

                    if (FrontendSettings.Multilingual)
                    {
                        ContentBody.Text = content.GetLocalized(c => c.Content);
                    }
                }
            }
        }
    }
}