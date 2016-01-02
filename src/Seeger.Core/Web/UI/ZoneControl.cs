using RazorEngine;
using RazorEngine.Templating;
using Seeger.Plugins;
using Seeger.Plugins.Widgets;
using Seeger.Templates;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

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

            if (!String.IsNullOrEmpty(locatedWidget.ViewName))
            {
                object model = null;

                if (widget.WidgetControllerType != null)
                {
                    var context = new WidgetContext
                    {
                        Page = locatedWidget.Page,
                        LocatedWidget = locatedWidget
                    };
                    var controller = (IWidgetController)Activator.CreateInstance(widget.WidgetControllerType);
                    controller.PreRender(context);

                    model = context.Model;
                }

                // render
                var view = widget.Views.Find(v => v.Name == locatedWidget.ViewName);
                var viewPath = Server.MapPath(UrlUtil.Combine(view.VirtualPath, "Default" + view.Extension));
                var viewContent = File.ReadAllText(viewPath, System.Text.Encoding.UTF8);
                var modelType = model == null ? null : model.GetType();
                var html = Engine.Razor.RunCompile(viewContent, widget.Plugin.Name + "." + widget.Name + "." + view.Name, modelType, model, null);

                var control = new LiteralControl(html);
                Controls.Add(control);
            }
            else
            {
                var control = WidgetControlLoader.Load(widget, Page, IsInDesignMode);

                Controls.Add(control);

                var widgetControl = control as WidgetControlBase;

                if (widgetControl == null)
                {
                    // Check if the widget control is using output cache
                    var cachingControl = control as PartialCachingControl;
                    if (cachingControl != null)
                    {
                        widgetControl = cachingControl.CachedControl as WidgetControlBase;
                    }
                }

                if (widgetControl != null)
                {
                    widgetControl.LocatedWidget = locatedWidget;
                    widgetControl.Widget = widget;
                    widgetControl.IsInDesignMode = IsInDesignMode;
                    widgetControl.WidgetAttributes.AddRange(locatedWidget.Attributes);
                }
            }
        }
    }
}
