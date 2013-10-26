using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Seeger.Data;
using Seeger.Plugins.Widgets;
using Seeger.Plugins;

namespace Seeger.Web.UI
{
    public class DesignerLayoutService
    {
        public PageItem CurrentPage { get; private set; }
        public CultureInfo DesignerCulture { get; private set; }
        public IEnumerable<LocatedWidgetViewModel> Widgets { get; private set; }

        public DesignerLayoutService(PageItem currentPage, CultureInfo designerCulture, IEnumerable<LocatedWidgetViewModel> widgets)
        {
            Require.NotNull(currentPage, "currentPage");
            Require.NotNull(designerCulture, "designerCulture");
            Require.NotNull(widgets, "widgets");

            this.CurrentPage = currentPage;
            this.DesignerCulture = designerCulture;
            this.Widgets = new List<LocatedWidgetViewModel>(widgets);
        }

        public void SaveLayoutChanges()
        {
            CurrentPage.ModifiedTime = DateTime.Now;

            foreach (var item in Widgets)
            {
                WidgetProcessEventArgs e = new WidgetProcessEventArgs
                {
                    CurrentPage = CurrentPage,
                    DesignerCulture = DesignerCulture,
                    LocatedWidgetViewModel = item
                };
                if (item.Id > 0)
                {
                    e.LocatedWidget = CurrentPage.LocatedWidgets.FirstOrDefault(it => it.Id == item.Id);
                }

                var plugin = PluginManager.FindEnabledPlugin(item.PluginName);

                if (plugin == null)
                    throw new InvalidOperationException("Cannot find plugin \"" + item.PluginName + "\" or it's not enabled.");
                
                var widget = plugin.FindWidget(item.WidgetName);

                if (widget == null)
                    throw new InvalidOperationException("Cannot find widget \"" + item.WidgetName + "\".");

                if (widget.WidgetProcessEventListener != null)
                {
                    widget.WidgetProcessEventListener.OnProcessing(e);
                }

                e.LocatedWidget = ProcessStateItem(item, widget);

                if (widget.WidgetProcessEventListener != null)
                {
                    widget.WidgetProcessEventListener.OnProcessed(e);
                }
            }

            Database.GetCurrentSession().Commit();
        }

        private LocatedWidget ProcessStateItem(LocatedWidgetViewModel locatedWidgetModel, WidgetDefinition widget)
        {
            var zone = CurrentPage.Layout.FindZone(locatedWidgetModel.ZoneName);

            if (zone == null)
                throw new InvalidOperationException("Zone \"" + locatedWidgetModel.ZoneName + "\" was not found.");

            LocatedWidget locatedWidget = null;

            if (locatedWidgetModel.State == WidgetState.Added)
            {
                locatedWidget = CurrentPage.AddWidgetToZone(zone, widget, locatedWidgetModel.Order);
            }
            else if (locatedWidgetModel.State == WidgetState.Removed)
            {
                locatedWidget = CurrentPage.LocatedWidgets.FirstOrDefault(it => it.Id == locatedWidgetModel.Id);

                if (locatedWidget == null)
                    throw new InvalidOperationException("Cannot find located widget with id: " + locatedWidgetModel.Id + ".");

                CurrentPage.LocatedWidgets.Remove(locatedWidget);
            }
            else if (locatedWidgetModel.State == WidgetState.Changed)
            {
                locatedWidget = CurrentPage.LocatedWidgets.FirstOrDefault(it => it.Id == locatedWidgetModel.Id);

                if (locatedWidget == null)
                    throw new InvalidOperationException("Cannot find located widget with id: " + locatedWidgetModel.Id + ".");

                locatedWidget.ZoneName = zone.Name;
                locatedWidget.Order = locatedWidgetModel.Order;
            }

            if (locatedWidgetModel.State != WidgetState.Removed)
            {
                foreach (var attr in locatedWidgetModel.Attributes)
                {
                    locatedWidget.Attributes.AddOrSet(attr.Key, attr.Value);
                }
            }

            return locatedWidget;
        }
    }
}
