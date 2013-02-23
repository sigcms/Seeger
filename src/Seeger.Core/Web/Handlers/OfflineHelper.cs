using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web;
using NHibernate.Linq;
using Seeger.Caching;
using Seeger.Globalization;
using Seeger.Data;

namespace Seeger.Web.Handlers
{
    class OfflineHelper
    {
        public static bool TryRedirectToOfflinePage()
        {
            string target = CmsVirtualPath.GetFull("/Offline.aspx");

            var frontendSettings = GlobalSettingManager.Instance.FrontendSettings;

            var session = NhSessionManager.GetCurrentSession();

            var offlinePage = session.Get<PageItem>(frontendSettings.OfflinePageId);
            if (offlinePage != null)
            {
                string path = HttpContext.Current.Request.Path;

                string extension = frontendSettings.PageExtension;

                if (frontendSettings.Multilingual)
                {
                    string cultureSegment = UrlUtility.GetFirstSegment(path);

                    if (!String.IsNullOrEmpty(cultureSegment))
                    {
                        var cache = FrontendLanguageCache.From(NhSessionManager.GetCurrentSession());

                        var language = cache.FindByName(cultureSegment);
                        if (language != null)
                        {
                            target = offlinePage.GetPagePath(CultureInfo.GetCultureInfo(language.Name), extension);
                        }
                        else
                        {
                            target = offlinePage.GetPagePath(CultureInfo.CurrentCulture, extension);
                        }
                    }
                    else
                    {
                        target = offlinePage.GetPagePath(CultureInfo.CurrentCulture, extension);
                    }
                }
                else
                {
                    target = offlinePage.GetPagePath(String.Empty, extension);
                }
            }

            if (!HttpContext.Current.Request.Path.IgnoreCaseStartsWith(target))
            {
                HttpContext.Current.Response.Redirect(target, true);
                return true;
            }

            return false;
        }
    }
}
