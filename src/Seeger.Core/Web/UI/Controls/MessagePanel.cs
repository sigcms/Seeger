using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI
{
    public class MessagePanel : Panel
    {
        public MessageType MessageType
        {
            get
            {
                return ViewState.TryGetValue<MessageType>("MessageType", MessageType.Info);
            }
            set
            {
                ViewState["MessageType"] = value;
            }
        }

        public string Text
        {
            get
            {
                return ViewState.TryGetValue<string>("Text", String.Empty);
            }
            set
            {
                ViewState["Text"] = value;
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(CssClass))
            {
                CssClass = "message " + MessageType.ToString().ToLower() + " " + CssClass;
            }
            else
            {
                CssClass = "message " + MessageType.ToString().ToLower();
            }

            base.RenderBeginTag(writer);

            writer.Write("<span class=\"message-icon\"></span>");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(Text))
            {
                writer.Write(Text);
            }
            else
            {
                base.RenderContents(writer);
            }
        }

    }
}
