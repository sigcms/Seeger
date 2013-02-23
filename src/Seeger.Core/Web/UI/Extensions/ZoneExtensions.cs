using Seeger.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public static class ZoneExtensions
    {
        public static ZoneControl LoadZoneControl(this Zone zone, LayoutPageBase page)
        {
            Require.NotNull(zone, "zone");
            Require.NotNull(page, "page");

            return page.GetZoneControls().FirstOrDefault(it => it.ZoneName == zone.Name);
        }

        public static DesignerZoneControl LoadDesigner(this Zone zone, PageDesignerBase page)
        {
            Require.NotNull(zone, "zone");
            Require.NotNull(page, "page");

            DesignerZoneControl designer = null;

            var holder = zone.LoadZoneControl(page);

            if (holder == null) return null;

            if (holder.Controls.Count == 0)
            {
                designer = new DesignerZoneControl();
                holder.Controls.Add(designer);
                designer.ZoneName = zone.Name;
            }
            else
            {
                designer = holder.Controls[0] as DesignerZoneControl;
                if (designer == null)
                {
                    throw new InvalidOperationException(
                        String.Format("Invalid state. The only child of the position holder for block '{0}' must be type of '{1}'.", zone.Name, typeof(DesignerZoneControl).FullName));
                }
            }

            return designer;
        }
    }
}
