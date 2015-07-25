using Seeger.Caching;
using Seeger.Data;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using NHibernate.Linq;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.Sample.Widgets.Widget2
{
    public partial class Default : WidgetControlBase
    {
        protected string PageUniqueName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //var page = PageCache.From(NhSession).FindPage(x => x.UniqueName == "OldOne");
            //PageUniqueName = page == null ? null : page.UniqueName;

            using (var session = Database.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var page = session.Query<PageItem>().FirstOrDefault();
                    PageUniqueName = page.UniqueName;
                    tx.Commit();
                }
            }
        }
    }
}