using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seeger.Caching;
using Seeger.Data;

namespace Seeger.Web.Handlers
{
    class RewriterIgnoredPathHandler : IRequestHandler
    {
        public static readonly RewriterIgnoredPathHandler Instance = new RewriterIgnoredPathHandler();

        public void Handle(RequestHandlerContext context)
        {
            bool ignore = false;

            foreach (var each in RewriterIgnoredPathCache.From(Database.GetCurrentSession()).RewriterIgnoredPaths)
            {
                if (each.Test(context.Request))
                {
                    ignore = true;
                    break;
                }
            }

            if (!ignore)
            {
                CustomRedirectHandler.Instance.Handle(context);
            }
        }
    }
}
