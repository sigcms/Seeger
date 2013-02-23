using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Seeger.Caching;
using Seeger.Globalization;

namespace Seeger.Web.Handlers
{
    class OfflineHandler : IRequestHandler
    {
        public static readonly OfflineHandler Instance = new OfflineHandler();

        public void Handle(RequestHandlerContext context)
        {
            if (GlobalSettingManager.Instance.FrontendSettings.IsWebsiteOffline)
            {
                if (!OfflineHelper.TryRedirectToOfflinePage())
                {
                    PageRequestHandler.Instance.Handle(context);
                }
            }
            else
            {
                RewriterIgnoredPathHandler.Instance.Handle(context);
            }
        }
    }
}
