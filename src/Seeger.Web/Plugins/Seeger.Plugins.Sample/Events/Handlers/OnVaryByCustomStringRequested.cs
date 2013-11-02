using Seeger.Events;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Sample.Events.Handlers
{
    public class OnVaryByCustomStringRequested : IHandle<Seeger.Web.Events.VaryByCustomStringRequested>
    {
        public void Handle(Web.Events.VaryByCustomStringRequested evnt)
        {
            var page = evnt.Context.CurrentHandler as LayoutPageBase;

            if (page != null)
            {
                var pageItem = page.PageItem;
                if (!String.IsNullOrEmpty(pageItem.BindedDomains))
                {
                    evnt.Result = pageItem.BindedDomains;
                }
            }
        }
    }
}