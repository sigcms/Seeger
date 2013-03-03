using Newtonsoft.Json;
using Seeger.Data;
using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Plugins.ImageSlider.Models;
using Seeger.Web;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Seeger.Plugins.ImageSlider
{
    public class WidgetProcessEventListener : IWidgetProcessEventListener
    {
        public void OnProcessing(WidgetProcessEventArgs e)
        {
        }

        public void OnProcessed(WidgetProcessEventArgs e)
        {
            var session = NhSessionManager.GetCurrentSession();

            if (e.StateItem.State == WidgetState.Removed)
            {
                var sliderId = e.WidgetInPage.Attributes.GetValue<int>("SliderId");
                if (sliderId > 0)
                {
                    session.Delete(session.Get<Slider>(sliderId));
                }
            }
            else
            {
                var data = e.StateItem.CustomData as IDictionary<string, object>;

                if (data == null || data.Count == 0) return;

                var json = data["ViewModelJson"] as string;

                if (String.IsNullOrEmpty(json)) return;

                var model = JsonConvert.DeserializeObject<SliderWidgetEditorViewModel>(json);

                var slider = model.Slider.Id > 0 ? session.Get<Slider>(model.Slider.Id) : new Slider();

                slider.Name = model.Slider.Name;

                // add new items or update existing items
                foreach (var item in model.Slider.Items)
                {
                    if (item.Id == 0)
                    {
                        slider.Items.Add(item);
                    }
                    else
                    {
                        var existing = slider.Items.FirstOrDefault(x => x.Id == item.Id);
                        existing.Caption = item.Caption;
                        existing.Description = item.Description;
                        existing.DisplayOrder = item.DisplayOrder;
                        existing.ImageUrl = item.ImageUrl;
                        existing.NavigateUrl = item.NavigateUrl;
                    }
                }

                // remove deleted items
                foreach (var item in slider.Items.Where(x => x.Id > 0).ToList())
                {
                    if (!model.Slider.Items.Any(x => x.Id == item.Id))
                    {
                        slider.Items.Remove(item);
                    }
                }

                if (slider.Id == 0)
                {
                    session.Save(slider);
                }

                // move temp images to final location

                var finalFolderUrl = "/Files/Slider/" + DateTime.Today.ToString("yyyy-MM");

                IOUtil.EnsureDirectoryCreated(HostingEnvironment.MapPath(finalFolderUrl));

                foreach (var item in slider.Items)
                {
                    if (item.ImageUrl.StartsWith("/Files/Temp/", StringComparison.OrdinalIgnoreCase))
                    {
                        var imageFileName = Path.GetFileName(item.ImageUrl);
                        var finalImageUrl = UrlUtility.Combine(finalFolderUrl, imageFileName);

                        File.Move(HostingEnvironment.MapPath(item.ImageUrl), HostingEnvironment.MapPath(finalImageUrl));
                        File.Move(HostingEnvironment.MapPath(item.ImageThumbUrl), HostingEnvironment.MapPath(SliderItem.GetThumbImageUrl(finalImageUrl)));

                        item.ImageUrl = finalImageUrl;
                    }
                }

                e.WidgetInPage.Attributes.AddOrSet("SliderId", slider.Id);
            }
        }
    }
}