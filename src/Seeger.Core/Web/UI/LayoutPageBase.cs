using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web.UI.WebControls;

using Seeger.Data;
using Seeger.Caching;
using System.IO;
using System.Web.UI;
using Seeger.Plugins;

namespace Seeger.Web.UI
{
    public abstract class LayoutPageBase : PageBase
    {
        public static readonly string QueryStringParam_PageId = "pageid";

        public int PageId
        {
            get
            {
                return Convert.ToInt32(Request.QueryString[QueryStringParam_PageId]);
            }
        }

        public string Suffix
        {
            get
            {
                return Request.QueryString["suffix"] ?? String.Empty;
            }
        }

        private SEOInfo _seoInfo;
        public SEOInfo SEOInfo
        {
            get
            {
                if (_seoInfo == null)
                {
                    _seoInfo = new SEOInfo();

                    // Try merge page seo settings
                    if (FrontendSettings.Multilingual)
                    {
                        var page = NhSession.Get<PageItem>(PageId);

                        var title = page.GetLocalized(p => p.PageTitle);
                        var keywords = page.GetLocalized(p => p.MetaKeywords);
                        var description = page.GetLocalized(p => p.MetaDescription);

                        _seoInfo.Merge(title, keywords, description);

                        var siteInfo = SiteInfoCache.From(NhSession).GetSiteInfo(CultureInfo.CurrentCulture);
                        if (siteInfo != null)
                        {
                            _seoInfo.Merge(siteInfo.SEOInfo);
                        }
                    }
                    else
                    {
                        _seoInfo.Merge(PageItem.PageTitle, PageItem.MetaKeywords, PageItem.MetaDescription);

                        var defaultSiteInfo = GlobalSettingManager.Instance.DefaultSiteInfo;
                        _seoInfo.Merge(defaultSiteInfo.PageTitle, defaultSiteInfo.MetaKeywords, defaultSiteInfo.MetaDescription);
                    }
                }

                return _seoInfo;
            }
        }

        private PageItem _pageItem;
        public PageItem PageItem
        {
            get
            {
                if (_pageItem == null)
                {
                    _pageItem = NhSession.Get<PageItem>(PageId);
                }
                return _pageItem;
            }
        }

        public IList<ZoneControl> GetZoneControls()
        {
            var controls = new List<ZoneControl>();
            foreach (var ctrl in this.GetControls(typeof(ZoneControl), true))
            {
                controls.Add((ZoneControl)ctrl);
            }

            return controls;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SetupWidgets();
            FixFromActionUrl();

            foreach (var interceptor in PageLifecycleInterceptors.Interceptors)
            {
                interceptor.OnInit(this);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var interceptor in PageLifecycleInterceptors.Interceptors)
            {
                interceptor.OnLoad(this);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupSeo();
            SetupTheme();

            foreach (var interceptor in PageLifecycleInterceptors.Interceptors)
            {
                interceptor.OnPreRender(this);
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            foreach (var interceptor in PageLifecycleInterceptors.Interceptors)
            {
                interceptor.OnUnload(this);
            }
        }

        private void SetupTheme()
        {
            Literal ctrl = new Literal();
            Header.Controls.Add(ctrl);
            ctrl.Text = HtmlHelper.LinkCssFiles(GetThemeFilePaths());
        }

        protected virtual void FixFromActionUrl()
        {
            Form.Action = Request.RawUrl;
        }

        protected virtual void SetupSeo()
        {
            if (!String.IsNullOrEmpty(SEOInfo.PageTitle))
            {
                Title = SEOInfo.PageTitle;
            }

            if (!String.IsNullOrEmpty(SEOInfo.MetaKeywords))
            {
                MetaKeywords = SEOInfo.MetaKeywords;
            }
            if (!String.IsNullOrEmpty(SEOInfo.MetaDescription))
            {
                MetaDescription = SEOInfo.MetaDescription;
            }
        }

        protected virtual IList<string> GetThemeFilePaths()
        {
            if (PageItem.Skin != null)
            {
                return PageItem.Skin.GetCssFileVirtualPaths(CultureInfo.CurrentUICulture);
            }

            return new List<string>();
        }

        protected virtual void SetupWidgets()
        {
            foreach (var zone in PageItem.Layout.Zones)
            {
                var widgetInPages = PageItem.WidgetInPages.Where(x => x.ZoneName == zone.Name);

                foreach (var setting in widgetInPages)
                {
                    var plugin = PluginManager.FindEnabledPlugin(setting.PluginName);
                    if (plugin == null) continue;

                    var widget = plugin.FindWidget(setting.WidgetName);
                    if (widget != null)
                    {
                        widget.TryAddToPage(this, zone, setting);
                    }
                }
            }
        }
    }
}
