using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;

using Seeger.Globalization;
using Seeger.Web.UI;
using Seeger.Caching;
using Seeger.Licensing;
using Seeger.Data;

namespace Seeger.Web.Handlers
{
    class PageRequestHandler : IRequestHandler
    {
        public static readonly PageRequestHandler Instance = new PageRequestHandler();

        public void Handle(RequestHandlerContext context)
        {
            if (context.TargetPath.Length == 0 || context.TargetPath == "/")
            {
                return;
            }

            var frontendSettings = GlobalSettingManager.Instance.FrontendSettings;

            string extension = frontendSettings.PageExtension;

            Url url = new Url(context.TargetPath);

            string[] segments = url.Segments.ToArray();
            if (url.FileExtension.IgnoreCaseEquals(extension))
            {
                var lastSegment = segments[segments.Length - 1];
                lastSegment = lastSegment.Substring(0, lastSegment.Length - extension.Length);
                segments[segments.Length - 1] = lastSegment;
            }

            var checkUnpublished = false;

            Boolean.TryParse(context.Request.QueryString["showUnpublished"], out checkUnpublished);

            var page = LongestMatch(PageCache.From(Database.GetCurrentSession()).RootPages, segments[0], checkUnpublished);

            if (page != null && segments.Length > 1)
            {
                for (int i = 1; i < segments.Length; i++)
                {
                    PageItem temp = LongestMatch(page.Pages, segments[i], checkUnpublished);
                    if (temp == null)
                    {
                        break;
                    }
                    else
                    {
                        page = temp;
                    }
                }
            }

            if (page != null)
            {
                string suffix = String.Empty;

                if (!String.IsNullOrEmpty(extension))
                {
                    if (!url.FileExtension.IgnoreCaseEquals(extension))
                    {
                        page = null;
                    }
                    else
                    {
                        suffix = url.Path.Substring(page.GetPagePathRelativeToCmsRoot(String.Empty, extension).Length - extension.Length);
                        suffix = suffix.Substring(0, suffix.Length - extension.Length);
                    }
                }
                else
                {
                    suffix = url.Path.Substring(page.GetPagePathRelativeToCmsRoot(String.Empty, extension).Length);
                }

                if (page != null)
                {
                    // Client may install other web systems(e.g., BBS) on the same server, 
                    // so we only validate license when we find the cms page to show
                    if (ValidateLicense() == false && page.Id != frontendSettings.OfflinePageId)
                    {
                        OfflineHelper.TryRedirectToOfflinePage();
                    }
                    else
                    {
                        string target = String.Format("{0}?{1}={2}&suffix={3}" + GetQueryString(),
                            page.Layout.AspxVirtualPath,
                            LayoutPageBase.QueryStringParam_PageId,
                            page.Id,
                            context.HttpContext.Server.UrlEncode(suffix));

                        context.HttpContext.RewritePath(target);
                    }
                }
            }
        }

        private bool ValidateLicense()
        {
            return LicensingService.CurrentLicense.IsValid
                && LicensingService.CurrentLicense.IsDomainLicensed(HttpContext.Current.Request.Url.Host);
        }

        private PageItem LongestMatch(IEnumerable<PageItem> pages, string segment, bool checkUnpublishedPages)
        {
            PageItem result = null;

            foreach (var each in pages)
            {
                if (checkUnpublishedPages || each.Published)
                {
                    if (each.Pages.Count > 0)
                    {
                        if (each.UrlSegment.IgnoreCaseEquals(segment))
                        {
                            result = each;
                            break;
                        }
                    }
                    else if (segment.IgnoreCaseStartsWith(each.UrlSegment))
                    {
                        if (result == null || each.UrlSegment.Length > result.UrlSegment.Length)
                        {
                            result = each;
                        }
                    }
                }
            }

            return result;
        }

        private string GetQueryString()
        {
            NameValueCollection queryStrings = new NameValueCollection(HttpContext.Current.Request.QueryString);

            queryStrings.Remove(LayoutPageBase.QueryStringParam_PageId);
            queryStrings.Remove("suffix");

            if (queryStrings.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var key in queryStrings.AllKeys)
                {
                    builder.Append("&").Append(key).Append("=").Append(HttpContext.Current.Server.UrlEncode(queryStrings[key]));
                }

                return builder.ToString();
            }

            return String.Empty;
        }

    }
}
