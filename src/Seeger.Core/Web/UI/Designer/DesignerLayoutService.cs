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

                var widget = PluginManager.FindEnabledPlugin(item.PluginName)
                                          .FindWidget(item.WidgetName);

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
            var block = CurrentPage.Layout.FindZone(locatedWidgetModel.ZoneName);

            LocatedWidget locatedWidget = null;

            if (locatedWidgetModel.State == WidgetState.Added)
            {
                locatedWidget = CurrentPage.AddWidgetToZone(block, widget, locatedWidgetModel.Order);
            }
            else if (locatedWidgetModel.State == WidgetState.Removed)
            {
                locatedWidget = CurrentPage.LocatedWidgets.FirstOrDefault(it => it.Id == locatedWidgetModel.Id);
                CurrentPage.LocatedWidgets.Remove(locatedWidget);
            }
            else if (locatedWidgetModel.State == WidgetState.Changed)
            {
                locatedWidget = CurrentPage.LocatedWidgets.FirstOrDefault(it => it.Id == locatedWidgetModel.Id);
                locatedWidget.ZoneName = block.Name;
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
