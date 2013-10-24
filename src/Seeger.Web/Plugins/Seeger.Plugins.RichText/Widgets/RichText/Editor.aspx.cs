using Seeger.Data;
using Seeger.Plugins.RichText.Domain;
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

namespace Seeger.Plugins.RichText.Widgets.RichText
{
    public partial class Editor : WidgetEditorBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod, ScriptMethod]
        public static string LoadContent(int id, string culture)
        {
            var db = Database.GetCurrentSession();
            var content = db.Get<TextContent>(id);
            var contentBody = content.Content;

            if (!String.IsNullOrEmpty(culture))
            {
                contentBody = content.GetLocalized(x => x.Content, CultureInfo.GetCultureInfo(culture));
            }

            return contentBody;
        }
    }
}