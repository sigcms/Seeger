using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin
{
    public partial class AdminMaster : System.Web.UI.MasterPage, IMessageProvider
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
    }
}