using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Seeger.Caching;
using Seeger.Data;

namespace Seeger.Web.Handlers
{
    class CustomRedirectHandler : IRequestHandler
    {
        public static readonly CustomRedirectHandler Instance = new CustomRedirectHandler();

        public void Handle(RequestHandlerContext context)
        {
            var redirect = CustomRedirectCache.From(Database.GetCurrentSession()).Match(context.Request);

            if (redirect != null)
            {
                if (redirect.RedirectMode == RedirectMode.Temporary)
                {
                    context.Response.Redirect(redirect.To, true);
                }
                else if (redirect.RedirectMode == RedirectMode.Permanent)
                {
                    context.Response.RedirectPermanent(redirect.To, true);
                }
            }
            else
            {
                PageRequestHandler.Instance.Handle(context);
            }
        }
    }
}
