using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public interface IMessageProvider
    {
        void ShowMessage(string message, MessageType type);

        void HideMessage();
    }
}
