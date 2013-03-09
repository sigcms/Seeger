using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web;

using Seeger.Caching;
using Seeger.Data;

namespace Seeger.Web.Handlers
{
    class HomeRequestHandler : IRequestHandler
    {
        public static readonly HomeRequestHandler Instance = new HomeRequestHandler();

        public void Handle(RequestHandlerContext context)
        {
            if (String.IsNullOrEmpty(context.TargetPath) || context.TargetPath == "/" || context.TargetPath.IgnoreCaseEquals("/default.aspx"))
            {
                var pageCache = PageCache.From(Database.GetCurrentSession());

                if (pageCache.Homepage != null && pageCache.Homepage.Published)
                {
                    context.TargetPath = pageCache.Homepage.GetPagePathRelativeToCmsRoot(String.Empty, GlobalSettingManager.Instance.FrontendSettings.PageExtension);
                }
                else
                {
                    throw new HttpException(404, "Page not found.");
                }
            }

            OfflineHandler.Instance.Handle(context);
        }
    }
}
