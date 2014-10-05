using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.Comments.Widgets.Comments
{
    public partial class Default : WidgetControlBase
    {
        protected Subject Subject { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Subject = SubjectContexts.Current.GetCurrentSubject(PageItem, new HttpContextWrapper(Context));
        }
    }
}