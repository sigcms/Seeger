using Newtonsoft.Json;
using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin
{
    public partial class Management : System.Web.UI.MasterPage, IMessageProvider
    {
        public void ShowMessage(string message, MessageType type)
        {
            Message.Visible = true;
            Message.MessageType = type;
            Message.Text = message;
        }

        public void HideMessage()
        {
            Message.Visible = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!String.IsNullOrEmpty(Page.Title))
            {
                PageTitle.Text = Page.Title;
            }
            else
            {
                PageTitlePanel.Visible = false;
            }
        }

        protected string GetGlobalResourceDirectoryJson()
        {
            var directory = ResourcesFolder.Global.GetResourceDictionary(CultureInfo.CurrentUICulture);
            return JsonConvert.SerializeObject(directory);
        }
    }
}