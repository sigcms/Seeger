using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class ZoneControl : System.Web.UI.WebControls.PlaceHolder
    {
        public string ZoneName
        {
            get { return ViewState["ZoneName"] as String ?? String.Empty; }
            set { ViewState["ZoneName"] = value; }
        }
    }
}
