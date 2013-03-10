using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.Processors
{
    public static class HttpProcessors
    {
        public static IEnumerable<IHttpProcessor> Processors { get; private set; }

        static HttpProcessors()
        {
            Processors = new List<IHttpProcessor>
            {
                new IgnoredPathProcessor(),
                new CustomRedirectProcessor(),
                new CultureProcessor(),
                new PageDomainBindingProcessor(),
                new HomepageProcessor(),
                new PageProcessor()
            };
        }

        public static void Process(HttpProcessingContext context)
        {
            foreach (var processor in Processors)
            {
                processor.Process(context);

                if (context.StopProcessing) break;
            }
        }
    }
}
