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
                        var title = PageItem.GetLocalized(p => p.PageTitle);
                        var keywords = PageItem.GetLocalized(p => p.MetaKeywords);
                        var description = PageItem.GetLocalized(p => p.MetaDescription);

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            FixFromActionUrl();

            foreach (var interceptor in PageLifecycleInterceptors.GetEnabledInterceptors())
            {
                interceptor.OnInit(this);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var interceptor in PageLifecycleInterceptors.GetEnabledInterceptors())
            {
                interceptor.OnLoad(this);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupSeo();
            IncludeSkinCssFiles();

            foreach (var interceptor in PageLifecycleInterceptors.GetEnabledInterceptors())
            {
                interceptor.OnPreRender(this);
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            foreach (var interceptor in PageLifecycleInterceptors.GetEnabledInterceptors())
            {
                interceptor.OnUnload(this);
            }
        }

        protected virtual void IncludeSkinCssFiles()
        {
            if (PageItem.Skin != null)
            {
                foreach (var path in PageItem.Skin.GetCssFileVirtualPaths(CultureInfo.CurrentUICulture))
                {
                    IncludeCssFile(path);
                }
            }
        }

        protected void IncludeCssFile(string path, string media = null)
        {
            var ctrl = new Literal
            {
                Text = HtmlHelper.IncludeCssFile(path, media)
            };
            Header.Controls.Add(ctrl);
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
    }
}
