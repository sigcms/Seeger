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
using NHibernate.Linq;

namespace Seeger.Plugins.RichText.Widgets.RichText
{
    public partial class Editor : WidgetEditorBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod, ScriptMethod]
        public static object LoadContent(int id, string culture)
        {
            var db = Database.GetCurrentSession();
            var content = db.Get<TextContent>(id);
            var contentTitle = content.Name;
            var contentBody = content.Content;

            if (!String.IsNullOrEmpty(culture))
            {
                contentTitle = content.GetLocalized(x => x.Name, CultureInfo.GetCultureInfo(culture));
                contentBody = content.GetLocalized(x => x.Content, CultureInfo.GetCultureInfo(culture));
            }

            return new
            {
                Title = contentTitle,
                Body = contentBody
            };
        }

        [WebMethod, ScriptMethod]
        public static object LoadExistingContents(string culture)
        {
            var db = Database.GetCurrentSession();
            var contents = db.Query<TextContent>()
                             .OrderByDescending(x => x.Id)
                             .ToList();

            var result = new List<object>();
            var cultureInfo = String.IsNullOrEmpty(culture) ? null : CultureInfo.GetCultureInfo(culture);

            foreach (var content in contents)
            {
                var title = content.Name;

                if (cultureInfo != null)
                {
                    title = content.GetLocalized(x => x.Name, cultureInfo) ?? title;
                }

                if (!String.IsNullOrEmpty(title))
                {
                    result.Add(new
                    {
                        content.Id,
                        Title = title
                    });
                }
            }

            return result;
        }
    }
}