using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Caching;
using System.Web;
using NHibernate.Linq;
using Seeger.Data;

namespace Seeger.Web
{
    public class FrontendEnvironment
    {
        public static string GetRootUrl(CultureInfo currentCulture)
        {
            if (currentCulture != null)
            {
                return GetRootUrl(currentCulture.Name);
            }

            return GetRootUrl(String.Empty);
        }

        public static string GetRootUrl(string currentCulture)
        {
            var requestUrl = HttpContext.Current.Request.Url;
            var portPart = (requestUrl.Port == 80) ? String.Empty : ":" + requestUrl.Port;
            
            var url = String.Empty;

            if (GlobalSettingManager.Instance.FrontendSettings.Multilingual && !String.IsNullOrEmpty(currentCulture))
            {
                var cache = FrontendLanguageCache.From(NhSessionManager.GetCurrentSession());

                var lang = cache.FindByName(currentCulture);
                if (lang != null && !String.IsNullOrEmpty(lang.BindedDomain))
                {
                    url = "http://" + lang.BindedDomain + portPart;
                }
                else
                {
                    url = "http://" + UrlUtility.Combine(requestUrl.Authority, CmsVirtualPath.GetFull(currentCulture));
                }
            }
            else
            {
                url = "http://" + requestUrl.Authority;
            }

            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            return url;
        }

        public static string GetPageUrl(CultureInfo targetCulture, PageItem page)
        {
            if (targetCulture != null)
            {
                return GetPageUrl(targetCulture.Name, page);
            }

            return GetPageUrl(String.Empty, page);
        }

        public static string GetPageUrl(string targetCulture, PageItem page)
        {
            Require.NotNull(page, "page");

            return GetFullUrl(targetCulture, page.GetPagePath(String.Empty, GlobalSettingManager.Instance.FrontendSettings.PageExtension));
        }

        public static string GetFullUrl(CultureInfo targetCulture, string pathWithoutCultureInfo)
        {
            if (targetCulture != null)
            {
                return GetFullUrl(targetCulture.Name, pathWithoutCultureInfo);
            }

            return GetFullUrl(String.Empty, pathWithoutCultureInfo);
        }

        public static string GetFullUrl(string targetCulture, string pathWithoutCultureInfo)
        {
            if (pathWithoutCultureInfo == null)
            {
                pathWithoutCultureInfo = String.Empty;
            }

            return UrlUtility.Combine(GetRootUrl(targetCulture), pathWithoutCultureInfo);
        }
    }
}
