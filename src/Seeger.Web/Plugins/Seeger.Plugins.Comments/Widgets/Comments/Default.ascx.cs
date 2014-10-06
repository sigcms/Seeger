using Seeger.Web;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.Comments.Widgets.Comments
{
    public partial class Default : WidgetControlBase
    {
        protected Subject Subject { get; private set; }

        protected bool IsCommenterAuthenticated { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsCommenterAuthenticated = AuthenticationContexts.Current.IsAuthenticated(new HttpContextWrapper(Context));
        }

        protected string RenderCommentBoxTemplate()
        {
            var templateName = IsCommenterAuthenticated ? "CommentBox_Authorized.ascx" : "CommentBox_Unauthorized.ascx";
            var templateVirtualPath = UrlUtil.Combine(Widget.VirtualPath, "Templates/Custom/" + templateName);

            if (!File.Exists(Server.MapPath(templateVirtualPath)))
            {
                templateVirtualPath = UrlUtil.Combine(Widget.VirtualPath, "Templates/" + templateName);
            }

            return ControlHelper.RenderControl(LoadControl(templateVirtualPath));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Subject = SubjectContexts.Current.GetCurrentSubject(PageItem, new HttpContextWrapper(Context));
        }
    }
}