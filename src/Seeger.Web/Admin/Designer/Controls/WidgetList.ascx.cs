using Seeger.Plugins;
using Seeger.Plugins.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Designer.Controls
{
    public partial class WidgetList : System.Web.UI.UserControl
    {
        public void Bind(IEnumerable<WidgetDefinition> widgets)
        {
            if (widgets.Any())
            {
                WidgetRepeater.DataSource = widgets;
                WidgetRepeater.DataBind();

                ContainerHolder.Visible = true;
            }
        }

        protected string GetIconUrl(object dataItem)
        {
            var widget = (WidgetDefinition)dataItem;
            if (String.IsNullOrEmpty(widget.IconUrl))
            {
                return "/Admin/Designer/widget-icon.png";
            }
            return widget.IconUrl;
        }

        protected bool IsEditorAutoOpen(object widgetDataItem)
        {
            return ((WidgetDefinition)widgetDataItem).EditorSettings.AutoOpen;
        }
    }
}