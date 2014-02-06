using Newtonsoft.Json;
using Seeger.Data;
using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Plugins.ImageSlider.Models;
using Seeger.Utils;
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
            var session = Database.GetCurrentSession();

            if (e.LocatedWidgetViewModel.State == WidgetState.Removed)
            {
                var sliderId = e.LocatedWidget.Attributes.GetValue<int>("SliderId");
                if (sliderId > 0)
                {
                    var slider = session.Get<Slider>(sliderId);
                    if (slider != null)
                    {
                        session.Delete(slider);
                    }
                }
            }
            else
            {
                var data = e.LocatedWidgetViewModel.CustomData;

                if (data == null || !data.HasValues) return;

                var json = data.Value<string>("ViewModel");

                if (String.IsNullOrEmpty(json)) return;

                var model = JsonConvert.DeserializeObject<SliderWidgetEditorViewModel>(json);

                var slider = model.Slider.Id > 0 ? session.Get<Slider>(model.Slider.Id) : new Slider();

                slider.Name = model.Slider.Name;
                slider.Width = model.Slider.Width;
                slider.Height = model.Slider.Height;
                slider.ShowNavigation = model.Slider.ShowNavigation;
                slider.ShowPagination = model.Slider.ShowPagination;

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

                e.LocatedWidget.Attributes.AddOrSet("SliderId", slider.Id);
            }
        }
    }
}