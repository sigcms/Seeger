using Seeger.Plugins.RichText.Domain;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.RichText.Widgets.RichText
{
    public partial class Editor : WidgetEditorBase
    {
        private TextContent _htmlContent;
        protected TextContent HtmlContent
        {
            get
            {
                if (_htmlContent == null)
                {
                    if (CurrentWidgetPersisted)
                    {
                        _htmlContent = NhSession.Get<TextContent>(WidgetInPage.Attributes.GetValue<int>("ContentId"));
                    }
                    else
                    {
                        _htmlContent = new TextContent();
                    }
                }
                return _htmlContent;
            }
        }

        protected int ContentId
        {
            get { return HtmlContent.Id; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindForm();
            }
        }

        private void BindForm()
        {
            if (FrontendSettings.Multilingual)
            {
                Content.Text = HtmlContent.GetLocalized(x => x.Content, PageCulture);
            }
            else
            {
                Content.Text = HtmlContent.Content;
            }
        }
    }
}