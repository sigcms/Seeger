using Seeger.Plugins;
using Seeger.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class ZoneControl : System.Web.UI.WebControls.PlaceHolder
    {
        public string ZoneName
        {
            get { return ViewState["ZoneName"] as String ?? String.Empty; }
            set { ViewState["ZoneName"] = value; }
        }

        public bool IsInDesignMode
        {
            get
            {
                return Page is PageDesignerBase;
            }
        }

        public PageItem PageItem
        {
            get
            {
                var page = Page as LayoutPageBase;
                if (page != null)
                {
                    return page.PageItem;
                }

                return null;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AddWidgetControls();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (IsInDesignMode)
            {
                writer.AddAttribute("class", "sig-zone");
                writer.AddAttribute("zone-name", ZoneName);
                writer.RenderBeginTag("div");
            }

            base.Render(writer);

            if (IsInDesignMode)
            {
                writer.Write("<div style=\"clear:both\"></div>");
                writer.RenderEndTag();
            }
        }

        private void AddWidgetControls()
        {
            var pageItem = PageItem;

            if (pageItem == null) return;

            var zone = pageItem.Layout.FindZone(ZoneName);

            foreach (var locatedWidget in pageItem.FindLocatedWidgetsByZone(ZoneName))
            {
                AddWidgetControl(locatedWidget, zone);
            }
        }

        private void AddWidgetControl(LocatedWidget locatedWidget, Zone zone)
        {
            var plugin = PluginManager.FindEnabledPlugin(locatedWidget.PluginName);
            if (plugin == null) return;

            var widget = plugin.FindWidget(locatedWidget.WidgetName);
            if (widget == null) return;

            if (IsInDesignMode)
            {
                var designer = widget.LoadDesigner(Page);
                designer.LocatedWidgetId = locatedWidget.Id;
                designer.WidgetDisplayOrder = locatedWidget.Order;
                designer.WidgetAttributes.AddRange(locatedWidget.Attributes);
                Controls.Add(designer);
            }
            else
            {
                var widgetControl = widget.LoadWidgetControl(Page);
                widgetControl.LocatedWidget = locatedWidget;
                Controls.Add(widgetControl);
            }
        }
    }
}
