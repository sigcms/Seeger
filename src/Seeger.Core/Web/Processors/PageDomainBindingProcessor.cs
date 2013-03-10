using Seeger.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.Processors
{
    public class PageDomainBindingProcessor : IHttpProcessor
    {
        public void Process(HttpProcessingContext context)
        {
            var domain = context.Request.Url.Host;
            var bindedPage = PageCache.From(context.NhSession).FindPageByDomain(domain);

            if (bindedPage != null)
            {
                context.MatchedPage = bindedPage;
            }
        }
    }
}
