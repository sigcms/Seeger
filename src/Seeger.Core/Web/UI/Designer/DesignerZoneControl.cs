using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class DesignerZoneControl : ZoneControl
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("class", "sig-zone");
            writer.AddAttribute("zone-name", ZoneName);
            writer.RenderBeginTag("div");
            base.Render(writer);
            writer.Write("<div style='clear:both'></div>");
            writer.RenderEndTag();
        }
    }
}
