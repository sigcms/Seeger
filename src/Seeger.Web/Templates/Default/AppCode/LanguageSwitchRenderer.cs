using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Globalization;
using Seeger.Caching;
using Seeger.Data;
using Seeger.Config;

namespace Seeger.Web.UI.Templates.Default
{
    public class LanguageSwitchRenderer
    {
        private FrontendLanguageCache _cache = FrontendLanguageCache.From(Database.GetCurrentSession());

        public CultureInfo Culture { get; private set; }

        public LanguageSwitchRenderer(CultureInfo culture)
        {
            Require.NotNull(culture, "culture");

            Culture = culture;
        }

        public string Render()
        {
            if (!GlobalSettingManager.Instance.FrontendSettings.Multilingual)
            {
                return String.Empty;
            }

            if (_cache.Languages.Count() < 2)
            {
                return String.Empty;
            }

            StringBuilder html = new StringBuilder();
            html.Append("<div class='language-switch'>");

            bool first = true;

            foreach (var lang in _cache.Languages)
            {
                if (!first)
                {
                    html.Append("|");
                }
                html.AppendFormat("<a class='language-link' href='{0}' title='{1}'>{1}</a>", GetLanguageLinkUrl(lang), lang.DisplayName, lang.DisplayName);

                first = false;
            }

            html.Append("</div>");

            return html.ToString();
        }

        private string GetLanguageLinkUrl(FrontendLanguage language)
        {
            string path = HttpContext.Current.Request.RawUrl;
            string culture = UrlUtil.GetFirstSegment(path);

            if (!String.IsNullOrEmpty(culture) && _cache.Contains(culture))
            {
                path = UrlUtil.RemoveFirstSegment(path);
            }

            return FrontendEnvironment.GetFullUrl(language.Name, path);
        }
    }
}