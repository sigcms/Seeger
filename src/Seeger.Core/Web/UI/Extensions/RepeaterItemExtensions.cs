using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Diagnostics.Contracts;

namespace Seeger.Web.UI
{
    public static class RepeaterItemExtensions
    {
        public static bool IsDataItem(this RepeaterItem item)
        {
            //Contract.Requires<ArgumentNullException>(item != null);

            return item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem;
        }
    }
}
