using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Seeger.Web.Processors;
using Seeger.Data;
using Seeger.Web.UI;
using System.Collections.Specialized;

namespace Seeger.Web
{
    public class HttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            var context = new HttpProcessingContext(new HttpContextWrapper(HttpContext.Current).Request, Database.GetCurrentSession());

            HttpProcessors.Process(context);

            if (context.Redirection != null && !String.IsNullOrEmpty(context.Redirection.RedirectUrl))
            {
                if (context.Redirection.RedirectMode == RedirectMode.Temporary)
                {
                    context.Response.Redirect(context.Redirection.RedirectUrl, true);
                }
                else
                {
                    context.Response.RedirectPermanent(context.Redirection.RedirectUrl, true);
                }
            }
            else if (context.MatchedPage != null)
            {
                var page = context.MatchedPage;
                var suffix = String.Join("/", context.RemainingSegments);

                if (!String.IsNullOrEmpty(suffix))
                {
                    suffix = HttpUtility.UrlEncode(suffix);
                }

                var url = String.Format("{0}?{1}={2}&suffix={3}{4}", 
                    page.Layout.AspxVirtualPath, LayoutPageBase.QueryStringParam_PageId, page.Id, suffix, SerializeQueryString(context.Request.QueryString));

                context.HttpContext.RewritePath(url);
            }
        }

        private string SerializeQueryString(NameValueCollection queryStrings)
        {
            if (queryStrings.Count == 0)
            {
                return String.Empty;
            }

            var result = new StringBuilder();

            foreach (var key in queryStrings.AllKeys)
            {
                if (String.IsNullOrEmpty(key))
                {
                    continue;
                }

                if (key.Equals("suffix", StringComparison.OrdinalIgnoreCase)
                    || key.Equals(LayoutPageBase.QueryStringParam_PageId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var value = queryStrings[key];

                if (!String.IsNullOrEmpty(value))
                {
                    value = HttpUtility.UrlEncode(value);
                }

                result.AppendFormat("&{0}={1}", key, value);
            }

            return result.ToString();
        }

        public void Dispose() { }
    }
}
