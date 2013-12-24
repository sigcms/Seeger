using Seeger.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seeger.Web.Processors
{
    public class CustomRedirectProcessor : IHttpProcessor
    {
        public void Process(HttpProcessingContext context)
        {
            var customRedirects = CustomRedirectCache.From(context.NhSession)
                                                     .CustomRedirects
                                                     .Where(x => x.IsEnabled);

            foreach (var customRedirect in customRedirects)
            {
                string destinationUrl;

                if (TryMatch(context, customRedirect, out destinationUrl))
                {
                    context.Redirection = new HttpRedirection
                    {
                        RedirectMode = customRedirect.RedirectMode,
                        RedirectUrl = destinationUrl
                    };

                    context.StopProcessing = true;

                    break;
                }
            }
        }

        private bool TryMatch(HttpProcessingContext context, CustomRedirect redirect, out string destinationUrl)
        {
            var originalUrl = context.Request.Url.ToString();

            destinationUrl = null;

            if (redirect.MatchByRegex)
            {
                if (Regex.IsMatch(originalUrl, redirect.From, RegexOptions.IgnoreCase))
                {
                    destinationUrl = Regex.Replace(originalUrl, redirect.From, redirect.To, RegexOptions.IgnoreCase);
                    return true;
                }
            }
            else
            {
                if (redirect.From.StartsWith("/"))
                {
                    originalUrl = context.Request.Url.PathAndQuery;
                }

                if (originalUrl.Equals(redirect.From, StringComparison.OrdinalIgnoreCase))
                {
                    destinationUrl = redirect.To;
                    return true;
                }
            }

            return false;
        }
    }
}
