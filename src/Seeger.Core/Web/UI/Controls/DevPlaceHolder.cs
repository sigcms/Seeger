using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI
{
    public class DevPlaceHolder : PlaceHolder
    {
        public override bool Visible
        {
            get
            {
                return CmsConfiguration.Instance.DevConfig.ShowDevPanels;
            }
            set
            {
                base.Visible = value;
            }
        }
    }
}
