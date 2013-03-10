using Seeger.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.Processors
{
    public class CustomRedirectProcessor : IHttpProcessor
    {
        public void Process(HttpProcessingContext context)
        {
            var redirect = CustomRedirectCache.From(context.NhSession).Match(context.Request);

            if (redirect == null) return;

            context.Redirection = new HttpRedirection
            {
                RedirectMode = redirect.RedirectMode,
                RedirectUrl = redirect.To
            };

            context.StopProcessing = true;
        }
    }
}
