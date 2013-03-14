using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Seeger.Web.UI.Admin
{
    public partial class Dashboard : AdminPageBase
    {
        protected string Greeting
        {
            get
            {
                string format = T("Dashboard.GreetingFormat");

                string greetingType = String.Empty;
                int hour = DateTime.Now.Hour;

                if (hour < 5)
                {
                    greetingType = T("Dashboard.Greeting_Evening");
                }
                else if (hour < 12)
                {
                    greetingType = T("Dashboard.Greeting_Morning");
                }
                else if (hour < 14)
                {
                    greetingType = T("Dashboard.Greeting_Midnoon");
                }
                else if (hour < 18)
                {
                    greetingType = T("Dashboard.Greeting_Afternoon");
                }
                else
                {
                    greetingType = T("Dashboard.Greeting_Evening");
                }

                return format.Replace("{Greeting}", greetingType)
                             .Replace("{UserName}", CurrentUser.Nick)
                             .Replace("{VersionNumber}", Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}