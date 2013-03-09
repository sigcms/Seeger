using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Globalization;
using System.Text;
using Seeger.Caching;

namespace Seeger.Web.UI.Admin.Pages.Controls
{
    public partial class PropertyPanel : AdminUserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected bool Multilingual
        {
            get { return FrontendSettings.Multilingual; }
        }

        protected string RenderCultureDropdown(string elementId)
        {
            var languageCache = FrontendLanguageCache.From(NhSession);

            if (!Multilingual || !languageCache.Languages.Any())  return String.Empty;

            StringBuilder html = new StringBuilder();
            html.AppendFormat("<ul id='{0}' class='dropdown-content'>", elementId);

            foreach (var lang in languageCache.Languages)
            {
                html.AppendFormat("<li><a href='#{0}'>{1}</a></li>", lang.Name, lang.DisplayName);
            }

            html.Append("</ul>");

            return html.ToString();
        }
    }
}