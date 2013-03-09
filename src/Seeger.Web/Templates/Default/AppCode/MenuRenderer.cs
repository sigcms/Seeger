using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seeger.Caching;
using System.Globalization;
using System.Text;
using Seeger.Data;

namespace Seeger.Web.UI.Templates.Default
{
    class MenuRenderer
    {
        private PageCache _pageCache;
        private FrontendSettings _frontendSettings;
        private FrontendLanguage _currentLanguage;

        public CultureInfo Culture { get; private set; }

        public MenuRenderer(CultureInfo culture)
        {
            Require.NotNull(culture, "culture");

            Culture = culture;

            _pageCache = PageCache.From(Database.GetCurrentSession());
            _frontendSettings = GlobalSettingManager.Instance.FrontendSettings;
            _currentLanguage = FrontendLanguageCache.From(Database.GetCurrentSession()).FindByName(culture.Name);
        }

        public string Render()
        {
            return Render(0);
        }

        public string Render(int activePageId)
        {
            var pages = _pageCache.RootPages.Where(it => it.Published && it.VisibleInMenu).ToList();

            if (pages.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<div class='menu'>");
                html.Append("<ul>");

                foreach (var page in pages)
                {
                    html.Append("<li><a");
                    if (page.Id == activePageId)
                    {
                        html.Append(" class='active'");
                    }
                    html.AppendFormat(" href='{0}' title='{1}'><span>{1}</span></a></li>", GetMenuUrl(page), GetMenuText(page));
                }

                html.Append("</ul>");
                html.Append("</div>");

                return html.ToString();
            }

            return String.Empty;
        }

        private string GetMenuUrl(PageItem page)
        {
            if (_pageCache.Homepage != null && page.Id == _pageCache.Homepage.Id)
            {
                return FrontendEnvironment.GetRootUrl(Culture);
            }

            return FrontendEnvironment.GetPageUrl(Culture, page);
        }

        private string GetMenuText(PageItem page)
        {
            if (!String.IsNullOrEmpty(page.MenuText))
            {
                return page.MenuText;
            }

            return page.DisplayName;
        }
    }
}