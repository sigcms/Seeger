using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Seeger.Web.Handlers;

namespace Seeger.Web
{
    public class UrlRewriteModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            PageDomainBindingHandler.Instance.Handle(new RequestHandlerContext(HttpContext.Current));
        }

        public void Dispose()
        {
        }
    }
}
