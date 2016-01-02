using Seeger.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seeger.Plugins.Widgets
{
    public class WidgetViewDefinition
    {
        public string Name { get; set; }

        public string Extension { get; set; }

        public string VirtualPath
        {
            get
            {
                return UrlUtil.Combine(Widget.ViewsFolderVirtualPath, Name);
            }
        }

        public WidgetDefinition Widget { get; set; }

        public WidgetViewDefinition(WidgetDefinition widget)
        {
            Widget = widget;
        }
    }
}
