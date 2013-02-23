using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Seeger.Web.UI.Admin
{
    public partial class Dashboard : BackendPageBase
    {
        protected string Greeting
        {
            get
            {
                string format = Localize("Dashboard.GreetingFormat");

                string greetingType = String.Empty;
                int hour = DateTime.Now.Hour;

                if (hour < 5)
                {
                    greetingType = Localize("Dashboard.Greeting_Evening");
                }
                else if (hour < 12)
                {
                    greetingType = Localize("Dashboard.Greeting_Morning");
                }
                else if (hour < 14)
                {
                    greetingType = Localize("Dashboard.Greeting_Midnoon");
                }
                else if (hour < 18)
                {
                    greetingType = Localize("Dashboard.Greeting_Afternoon");
                }
                else
                {
                    greetingType = Localize("Dashboard.Greeting_Evening");
                }

                return format.Replace("{Greeting}", greetingType)
                             .Replace("{UserName}", CurrentUser.UserName)
                             .Replace("{VersionNumber}", Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}