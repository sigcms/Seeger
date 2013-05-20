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

            // Check lanuage by domain
            var language = languages.FindByDomain(context.Request.Url.Host);

            // check lanage by first url segment
            if (language == null && context.RemainingSegments.Count > 0)
            {
                var cultureName = context.RemainingSegments[0];
                language = languages.FindByName(cultureName);

                if (language != null)
                {
                    context.RemainingSegments.RemoveAt(0);
                }
            }

            // Check user browser's language
            if (language == null && context.Request.UserLanguages != null && context.Request.UserLanguages.Length > 0)
            {
                foreach (var name in context.Request.UserLanguages)
                {
                    language = languages.FindByName(name);

                    if (language != null)
                    {
                        break;
                    }
                }
            }

            // use default language
            if (language == null)
            {
                language = languages.Languages.FirstOrDefault();
            }

            if (language != null)
            {
                context.Culture = CultureInfo.GetCultureInfo(language.Name);
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = context.Culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = context.Culture;
        }
    }
}
