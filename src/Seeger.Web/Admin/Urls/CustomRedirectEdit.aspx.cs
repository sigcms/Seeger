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
    public partial class CustomRedirectEdit : DetailPageBase<CustomRedirect>
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "CustomRedirect", (FormState == UI.FormState.AddItem) ? "Add" : "Edit");
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
            Response.Redirect("CustomRedirectList.aspx");
        }

        protected override void BindSubmitEventHandler(EventHandler handler)
        {
            SubmitButton.Click += handler;
        }

        public override void InitView(CustomRedirect entity)
        {
            RedirectMode.SelectedValue = entity.RedirectMode.ToString();
            From.Text = entity.From;
            To.Text = entity.To;
            MatchByRegex.Checked = entity.MatchByRegex;
        }

        public override void UpdateObject(CustomRedirect entity)
        {
            entity.RedirectMode = (RedirectMode)Enum.Parse(typeof(RedirectMode), RedirectMode.SelectedValue);
            entity.From = From.Text.Trim();
            entity.To = To.Text.Trim();
            entity.MatchByRegex = MatchByRegex.Checked;
        }
    }
}