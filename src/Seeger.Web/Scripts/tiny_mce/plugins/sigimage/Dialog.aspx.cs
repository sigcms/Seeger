using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Globalization;

namespace Seeger.Web.UI.Scripts.tiny_mce.plugins.sigimage
{
    public partial class Dialog : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (AdminSession.Current == null)
            {
                Response.End();
            }
        }

        protected string T(string key)
        {
            return ResourcesFolder.Global.GetValue(key, AdminSession.Current.UICulture);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = T("SigImage.DialogTitle");
        }
    }
}