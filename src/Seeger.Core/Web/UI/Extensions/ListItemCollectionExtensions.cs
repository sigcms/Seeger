using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI
{
    public static class ListItemCollectionExtensions
    {
        public static bool Any(this ListItemCollection items, Func<ListItem, bool> predicate)
        {
            foreach (var item in items)
            {
                if (predicate((ListItem)item))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool All(this ListItemCollection items, Func<ListItem, bool> predicate)
        {
            foreach (var item in items)
            {
                if (!predicate((ListItem)item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
