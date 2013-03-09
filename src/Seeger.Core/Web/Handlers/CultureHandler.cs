using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using NHibernate.Linq;
using Seeger.Licensing;
using Seeger.Data;
using Seeger.Caching;

namespace Seeger.Web.Handlers
{
    class CultureHandler : IRequestHandler
    {
        public static readonly CultureHandler Instance = new CultureHandler();

        public void Handle(RequestHandlerContext context)
        {
            var frontendSettings = GlobalSettingManager.Instance.FrontendSettings;

            if (frontendSettings.Multilingual)
            {
                var languages = FrontendLanguageCache.From(Database.GetCurrentSession());

                var language = languages.FindByDomain(context.Request.Url.Host);

                if (language != null)
                {
                    SetupCulture(CultureInfo.GetCultureInfo(language.Name));
                }
                else
                {
                    string firstSegment = UrlUtility.GetFirstSegment(context.TargetPath);

                    if (!String.IsNullOrEmpty(firstSegment) && languages.Contains(firstSegment))
                    {
                        SetupCulture(CultureInfo.GetCultureInfo(firstSegment));

                        if (context.TargetPath.Length > firstSegment.Length)
                        {
                            context.TargetPath = context.TargetPath.Substring(firstSegment.Length + 1);
                        }
                        else
                        {
                            context.TargetPath = "/";
                        }
                    }
                    else
                    {
                        var defaultLanguage = frontendSettings.DefaultLanguage;

                        if (!String.IsNullOrEmpty(defaultLanguage))
                        {
                            SetupCulture(CultureInfo.GetCultureInfo(defaultLanguage));
                        }
                        else
                        {
                            string acceptLanguage = context.Request.Headers["Accept-Language"];
                            if (!String.IsNullOrEmpty(acceptLanguage))
                            {
                                foreach (var langName in acceptLanguage.SplitWithoutEmptyEntries(',').Select(it => it.Trim()))
                                {
                                    if (!String.IsNullOrEmpty(langName))
                                    {
                                        var lang = languages.FindByName(langName);
                                        if (lang != null)
                                        {
                                            SetupCulture(CultureInfo.GetCultureInfo(lang.Name));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            HomeRequestHandler.Instance.Handle(context);
        }

        private static void SetupCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
