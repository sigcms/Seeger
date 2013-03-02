using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Seeger.Web.UI
{
    public static class ControlHelper
    {
        public static Control LoadControl(string virtualPath)
        {
            var page = new Page();
            return page.LoadControl(virtualPath);
        }

        public static string RenderControl(Control control)
        {
            using (var writer = new StringWriter())
            using (var htmlWriter = new HtmlTextWriter(writer))
            {
                control.RenderControl(htmlWriter);
                htmlWriter.Flush();

                return writer.ToString();
            }
        }
    }
}
