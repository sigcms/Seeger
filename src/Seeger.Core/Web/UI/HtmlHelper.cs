using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class HtmlHelper
    {
        public static string ScriptForCms(string virtualPathRelativeToCmsRoot)
        {
            return String.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", virtualPathRelativeToCmsRoot);
        }

        public static string LinkCssFiles(IEnumerable<string> cssFilePaths, string media = null)
        {
            var html = new StringBuilder();

            foreach (var path in cssFilePaths)
            {
                html.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\"", path);

                if (!String.IsNullOrEmpty(media))
                {
                    html.AppendFormat(" media=\"{0}\"", media);
                }

                html.Append(" />");
            }

            return html.ToString();
        }
    }
}
