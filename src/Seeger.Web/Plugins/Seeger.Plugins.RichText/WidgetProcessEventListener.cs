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
                session.Delete(session.Get<TextContent>(e.LocatedWidget.Attributes.GetValue<int>("contentId")));
            }
            else
            {
                var data = e.LocatedWidgetViewModel.CustomData;

                if (data != null)
                {
                    TextContent content = null;

                    var name = data.Value<string>("name") ?? String.Empty;
                    var body = data.Value<string>("content") ?? String.Empty;
                    var isContentChanged = data.Value<bool>("isContentChanged");

                    if (e.LocatedWidgetViewModel.State == WidgetState.Changed)
                    {
                        var contentId = e.LocatedWidget.Attributes.GetValue<int>("ContentId");
                        content = session.Get<TextContent>(contentId);
                        content.Name = name;
                    }
                    else
                    {
                        content = new TextContent();
                        content.Name = name;
                        session.Save(content);

                        e.LocatedWidget.Attributes.Add("ContentId", content.Id);
                    }

                    if (isContentChanged)
                    {
                        if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
                        {
                            content.SetLocalized(c => c.Content, body, e.DesignerCulture);
                        }
                        else
                        {
                            content.Content = body;
                        }
                    }
                }
            }
        }
    }
}