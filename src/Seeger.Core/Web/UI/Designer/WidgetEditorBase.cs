using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Caching;
using Seeger.Plugins.Widgets;
using Seeger.Plugins;
using Seeger.Templates;

namespace Seeger.Web.UI
{
    public class WidgetEditorBase : AdminPageBase
    {
        private string WidgetPluginName
        {
            get { return Request.QueryString["widgetPlugin"]; }
        }

        private WidgetDefinition _widget;
        protected WidgetDefinition Widget
        {
            get
            {
                if (_widget == null)
                {
                    var plugin = PluginManager.FindEnabledPlugin(WidgetPluginName);
                    _widget = plugin.FindWidget(Request.QueryString["widgetName"]);
                }
                return _widget;
            }
        }

        private Zone _containingZone;
        protected Zone ContainingZone
        {
            get
            {
                if (_containingZone == null)
                {
                    string zoneName = Request.QueryString["zoneName"];
                    _containingZone = PageItem.Layout.FindZone(zoneName);
                }
                return _containingZone;
            }
        }

        private CultureInfo _pageCulture;
        protected CultureInfo PageCulture
        {
            get
            {
                if (_pageCulture == null && !String.IsNullOrEmpty(Request.QueryString["culture"]))
                {
                    _pageCulture = CultureInfo.GetCultureInfo(Request.QueryString["culture"]);
                }
                return _pageCulture;
            }
        }

        private int _widgetInPageId = -1;
        protected int WidgetInPageId
        {
            get
            {
                if (_widgetInPageId < 0)
                {
                    _widgetInPageId = Convert.ToInt32(Request.QueryString["widgetInPageId"]);
                }
                return _widgetInPageId;
            }
        }

        private WidgetInPage _widgetInPage;
        protected WidgetInPage WidgetInPage
        {
            get
            {
                if (_widgetInPage == null)
                {
                    if (CurrentWidgetPersisted)
                    {
                        _widgetInPage = PageItem.GetWidget(WidgetInPageId);
                    }
                    else
                    {
                        _widgetInPage = PageItem.AddWidget(ContainingZone, Widget);
                    }
                }
                return _widgetInPage;
            }
        }

        protected EntityAttributeCollection WidgetAttributes
        {
            get { return WidgetInPage.Attributes; }
        }

        public bool CurrentWidgetPersisted
        {
            get { return WidgetInPageId > 0; }
        }

        private PageItem _pageItem;
        protected PageItem PageItem
        {
            get
            {
                if (_pageItem == null)
                {
                    _pageItem = NhSession.Get<PageItem>(Convert.ToInt32(Request.QueryString["pageid"]));
                }
                return _pageItem;
            }
        }

        protected override string T(string key, CultureInfo culture)
        {
            return Widget.Localize(key, culture, true);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ClientScript.RegisterClientScriptBlock(GetType(), "EditorContext", "window.editorContext = window.top.Sig.EditorContext.get_current();", true);
        }
    }
}
