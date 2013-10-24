using Seeger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using NHibernate.Linq;

namespace Seeger.Web.UI.Admin.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class DesignerService : System.Web.Services.WebService
    {
        [WebMethod]
        public object LoadWidgetAttributes(int[] locatedWidgetIds)
        {
            var db = Database.GetCurrentSession();
            var locatedWidgets = db.Query<LocatedWidget>()
                                   .Where(x => locatedWidgetIds.Contains(x.Id))
                                   .ToList();

            var result = new Dictionary<string, Dictionary<string, string>>();

            foreach (var locatedWidget in locatedWidgets)
            {
                // Must stringify the id, otherwise jsonerror will be reported by asp.net webservice
                result.Add(locatedWidget.Id.ToString(), locatedWidget.Attributes.ToDictionary());
            }

            return result;
        }
    }
}
