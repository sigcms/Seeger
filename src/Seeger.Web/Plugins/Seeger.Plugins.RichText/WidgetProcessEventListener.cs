using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Seeger.Data;
using Seeger.Web.UI;
using Seeger.Globalization;
using Seeger.Plugins.RichText.Domain;
using Seeger.Config;

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

            if (e.LocatedWidgetViewModel.State == WidgetState.Removed)
            {
                var contentId = e.LocatedWidget.Attributes.GetValue<int>("contentId");
                var content = session.Get<TextContent>(contentId);

                if (content != null)
                {
                    session.Delete(content);
                }
            }
            else
            {
                var data = e.LocatedWidgetViewModel.CustomData;

                if (data != null)
                {
                    TextContent content = null;

                    var title = data.Value<string>("title");
                    var body = data.Value<string>("content");

                    // If the widget is just dragged to another location, both title and body will be null.
                    // So we shouldn't change the TextContent instance's Name and Body in this case,
                    // otherwise the original text will be lost unexpected.
                    if (title == null && body == null)
                    {
                        return;
                    }

                    var contentId = e.LocatedWidget.Attributes.GetValue<int>("ContentId");

                    if (contentId > 0)
                    {
                        content = session.Get<TextContent>(contentId);
                    }
                    
                    if (content == null)
                    {
                        content = new TextContent();
                        session.Save(content);
                        e.LocatedWidget.Attributes.AddOrSet("ContentId", content.Id);
                    }

                    if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
                    {
                        if (title != null)
                        {
                            content.SetLocalized(c => c.Name, title, e.DesignerCulture);
                        }
                        if (body != null)
                        {
                            content.SetLocalized(c => c.Content, body, e.DesignerCulture);
                        }
                    }
                    else
                    {
                        if (title != null)
                        {
                            content.Name = title;
                        }
                        if (body != null)
                        {
                            content.Content = body;
                        }
                    }
                }
            }
        }
    }
}