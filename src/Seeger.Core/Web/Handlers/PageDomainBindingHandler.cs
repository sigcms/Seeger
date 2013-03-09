using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Caching;
using Seeger.Data;

namespace Seeger.Web.Handlers
{
    class PageDomainBindingHandler : IRequestHandler
    {
        public static readonly PageDomainBindingHandler Instance = new PageDomainBindingHandler();

        public void Handle(RequestHandlerContext context)
        {
            var domain = context.Request.Url.Host;
            var page = PageCache.From(Database.GetCurrentSession()).FindPageByDomain(domain);

            // TODO: What if requesting assets (jpg, swf) ?
            if (page != null)
            {
                var path = page.GetPagePath(String.Empty);

                if (!String.IsNullOrEmpty(InstallationInfo.InstallPath.Trim('/')))
                {
                    path = "/" + UrlUtility.Combine(InstallationInfo.InstallPath.Trim('/'), path);
                }

                if (String.IsNullOrEmpty(context.TargetPath))
                {
                    path += GlobalSettingManager.Instance.FrontendSettings.PageExtension;
                }

                context.TargetPath = UrlUtility.Combine(path, context.TargetPath);
            }

            InstallPathHandler.Instance.Handle(context);
        }
    }
}
