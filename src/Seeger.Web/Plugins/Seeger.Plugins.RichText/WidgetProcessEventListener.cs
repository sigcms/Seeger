using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Seeger.Data;
using Seeger.Web.UI;
using Seeger.Globalization;
using Seeger.Plugins.RichText.Domain;

namespace Seeger.Plugins.RichText
{
    public class WidgetProcessEventListener : IWidgetProcessEventListener
    {
        public void OnProcessing(WidgetProcessEventArgs e)
        {
        }

        public void OnProcessed(WidgetProcessEventArgs e)
        {
            var session = Database.GetCurrentSession();

            if (e.StateItem.State == WidgetState.Removed)
            {
                session.Delete(session.Get<TextContent>(e.WidgetInPage.Attributes.GetValue<int>("contentId")));
            }
            else
            {
                var data = e.StateItem.CustomData as IDictionary<string, object>;

                if (data != null && data.Count > 0)
                {
                    TextContent content = null;

                    if (e.StateItem.State == WidgetState.Changed)
                    {
                        content = session.Get<TextContent>(Convert.ToInt32(data["contentId"]));
                        content.Name = data["name"].AsString();
                    }
                    else
                    {
                        content = new TextContent();
                        content.Name = data["name"].AsString();
                        session.Save(content);

                        e.WidgetInPage.Attributes.Add("ContentId", content.Id);
                    }

                    if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
                    {
                        content.SetLocalized(c => c.Content, data["content"].AsString(), e.DesignerCulture);
                    }
                    else
                    {
                        content.Content = data["content"].AsString();
                    }
                }
            }
        }
    }
}