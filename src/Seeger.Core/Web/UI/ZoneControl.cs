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
                return LayoutPage != null && LayoutPage.IsInDesignMode;
            }
        }

        public LayoutPageBase LayoutPage
        {
            get
            {
                return Page as LayoutPageBase;
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

            var control = WidgetControlLoader.Load(widget, Page, IsInDesignMode);

            var widgetControl = control as WidgetControlBase;
            if (widgetControl != null)
            {
                widgetControl.LocatedWidget = locatedWidget;
                widgetControl.Widget = widget;
                widgetControl.WidgetAttributes.AddRange(locatedWidget.Attributes);
            }

            Controls.Add(control);
        }
    }
}
