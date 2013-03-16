using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class HtmlHelper
    {
        public static string IncludeCssFiles(IEnumerable<string> paths, string media = null)
        {
            var html = new StringBuilder();

            foreach (var path in paths)
            {
                html.AppendLine(IncludeCssFile(path, media));
            }

            return html.ToString();
        }

        public static string IncludeCssFile(string path, string media = null)
        {
            return String.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\"{1} />",
                path, String.IsNullOrEmpty(media) ? String.Empty : " media=\"" + media + "\"");
        }
    }
}
