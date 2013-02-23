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

            if (AdministrationSession.Current == null)
            {
                Response.End();
            }
        }

        protected string Localize(string key)
        {
            return ResourcesFolder.Global.GetValue(key, AdministrationSession.Current.UICulture);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Localize("SigImage.DialogTitle");
        }
    }
}