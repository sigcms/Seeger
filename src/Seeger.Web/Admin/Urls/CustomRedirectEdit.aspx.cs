using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Data;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Urls
{
    public partial class CustomRedirectEdit : AdminPageBase
    {
        protected int RedirectId
        {
            get
            {
                return Request.QueryString.TryGetValue<int>("id", 0);
            }
        }

        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "CustomRedirect", (RedirectId == 0) ? "Add" : "Edit");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (RedirectId > 0)
                {
                    InitView(NhSession.Get<CustomRedirect>(RedirectId));
                }
            }
        }

        public void InitView(CustomRedirect entity)
        {
            RedirectMode.SelectedValue = entity.RedirectMode.ToString();
            UrlMatchMode.SelectedValue = entity.UrlMatchMode.ToString();
            From.Text = entity.From;
            To.Text = entity.To;
            MatchByRegex.Checked = entity.MatchByRegex;
            IsEnabled.Checked = entity.IsEnabled;
        }

        public void UpdateObject(CustomRedirect entity)
        {
            entity.RedirectMode = (RedirectMode)Enum.Parse(typeof(RedirectMode), RedirectMode.SelectedValue);
            entity.From = From.Text.Trim();
            entity.To = To.Text.Trim();
            entity.UrlMatchMode = (UrlMatchMode)Enum.Parse(typeof(UrlMatchMode), UrlMatchMode.SelectedValue);
            entity.MatchByRegex = MatchByRegex.Checked;
            entity.IsEnabled = IsEnabled.Checked;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (RedirectId > 0)
            {
                var redirect = NhSession.Get<CustomRedirect>(RedirectId);
                UpdateObject(redirect);
            }
            else
            {
                var redirect = new CustomRedirect();
                UpdateObject(redirect);
                NhSession.Save(redirect);
            }

            NhSession.Commit();

            Response.Redirect("CustomRedirectList.aspx");
        }
    }
}