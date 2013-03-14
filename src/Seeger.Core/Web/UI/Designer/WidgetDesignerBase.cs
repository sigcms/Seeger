using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using Seeger.Plugins.Widgets;
using Seeger.Plugins;

namespace Seeger.Web.UI
{
    public class WidgetDesignerBase : InDesignerUerControlBase
    {
        public WidgetDesignerBase()
        {
            WidgetAttributes = new EntityAttributeCollection();
        }

        public string WidgetName
        {
            get { return ViewState["WidgetName"] as String ?? String.Empty; }
            set { ViewState["WidgetName"] = value; }
        }

        public string PluginName
        {
            get { return ViewState["PluginName"] as String ?? String.Empty; }
            set { ViewState["PluginName"] = value; }
        }

        public string TemplateName
        {
            get { return ViewState["TemplateName"] as String ?? String.Empty; }
            set { ViewState["TemplateName"] = value; }
        }

        public string WidgetInPageId
        {
            get { return ViewState["WidgetInPageId"] as String ?? String.Empty; }
            set { ViewState["WidgetInPageId"] = value; }
        }

        public int WidgetDisplayOrder
        {
            get { return ViewState.TryGetValue<int>("WidgetDisplayOrder", -1); }
            set { ViewState["WidgetDisplayOrder"] = value; }
        }

        public virtual Unit DesignerWidth
        {
            get { return Unit.Empty; }
        }

        public virtual Unit DesignerHeight
        {
            get { return Unit.Empty; }
        }

        public EntityAttributeCollection WidgetAttributes { get; private set; }

        private WidgetDefinition _widget;

        public WidgetDefinition Widget
        {
            get
            {
                if (_widget == null)
                {
                    var plugin = PluginManager.FindEnabledPlugin(PluginName);
                    _widget = plugin.FindWidget(WidgetName);
                }
                
                return _widget;
            }
        }

        protected override string T(string key, CultureInfo culture)
        {
            if (culture == null)
            {
                return Widget.Localize(key, CultureInfo.CurrentUICulture, true);
            }

            return Widget.Localize(key, culture, true);
        }

        public override void RenderControl(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("class", "sig-widget");
            writer.AddAttribute("plugin-name", PluginName);
            writer.AddAttribute("template-name", TemplateName);
            writer.AddAttribute("widget-name", WidgetName);
            writer.AddAttribute("widget-in-page-id", WidgetInPageId);
            writer.AddAttribute("editable", Widget.Editable.ToString().ToLower());
            writer.AddAttribute("editor-width", Widget.EditorSettings.Width.ToString());
            writer.AddAttribute("editor-height", Widget.EditorSettings.Height.ToString());

            if (WidgetDisplayOrder >= 0)
            {
                writer.AddAttribute("order", WidgetDisplayOrder.ToString());
            }

            string style = String.Empty;
            if (!DesignerWidth.IsEmpty)
            {
                style = String.Format("width:{0};", DesignerWidth.ToString());
            }
            if (!DesignerHeight.IsEmpty)
            {
                style += String.Format("height:{0};", DesignerHeight.ToString());
            }
            if (style.Length > 0)
            {
                writer.AddAttribute("style", style);
            }

            writer.RenderBeginTag("div");
            base.RenderControl(writer);
            writer.RenderEndTag();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
            RenderWidgetContent(writer);
        }

        protected virtual void RenderWidgetContent(System.Web.UI.HtmlTextWriter writer) { }
    }
}
