using Seeger.Caching;
using Seeger.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Seeger.Web.Processors
{
    public class CultureProcessor : IHttpProcessor
    {
        public void Process(HttpProcessingContext context)
        {
            var frontendSettings = GlobalSettingManager.Instance.FrontendSettings;

            if (!frontendSettings.Multilingual) return;

            var languages = FrontendLanguageCache.From(context.NhSession);
            var language = languages.FindByDomain(context.Request.Url.Host);

            if (language != null)
            {
                context.Culture = CultureInfo.GetCultureInfo(language.Name);
                return;
            }

            if (context.RemainingSegments.Count == 0) return;

            var cultureName = context.RemainingSegments[0];

            if (languages.Contains(cultureName))
            {
                context.Culture = CultureInfo.GetCultureInfo(cultureName);
                context.RemainingSegments.RemoveAt(0);
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = context.Culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = context.Culture;
        }
    }
}
