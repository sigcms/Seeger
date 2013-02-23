using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seeger.Data;
using Seeger.Web.UI.DataManagement;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Urls
{
    public partial class ReservedPathEdit : DetailPageBase<RewriterIgnoredPath>
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "RewriterIgnoredPath", (FormState == UI.FormState.AddItem) ? "Add" : "Edit");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override object CreateKey(string keyStringValue)
        {
            return Convert.ToInt32(keyStringValue);
        }

        protected override void OnSubmitted()
        {
            Response.Redirect("RewriterIgnoredPathList.aspx");
        }

        protected override void BindSubmitEventHandler(EventHandler handler)
        {
            SubmitButton.Click += handler;
        }

        public override void InitView(RewriterIgnoredPath entity)
        {
            if (!String.IsNullOrEmpty(entity.Name))
            {
                Name.Text = entity.Name;
            }
            Path.Text = entity.Path;
            MatchByRegex.Checked = entity.MatchByRegex;
        }

        public override void UpdateObject(RewriterIgnoredPath entity)
        {
            entity.Name = Name.Text.Trim();
            entity.Path = Path.Text.Trim();
            entity.MatchByRegex = MatchByRegex.Checked;
        }
    }
}