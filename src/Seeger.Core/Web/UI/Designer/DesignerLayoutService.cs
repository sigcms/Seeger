using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Seeger.Data;
using Seeger.Plugins.Widgets;

namespace Seeger.Web.UI
{
    public class DesignerLayoutService
    {
        public PageItem CurrentPage { get; private set; }
        public CultureInfo DesignerCulture { get; private set; }
        public IEnumerable<WidgetStateItem> StateItems { get; private set; }

        public DesignerLayoutService(PageItem currentPage, CultureInfo designerCulture, IEnumerable<WidgetStateItem> stateItems)
        {
            Require.NotNull(currentPage, "currentPage");
            Require.NotNull(designerCulture, "designerCulture");
            Require.NotNull(stateItems, "stateItems");

            this.CurrentPage = currentPage;
            this.DesignerCulture = designerCulture;
            this.StateItems = new List<WidgetStateItem>(stateItems);
        }

        public void SaveLayoutChanges()
        {
            CurrentPage.ModifiedTime = DateTime.Now;

            foreach (var item in this.StateItems)
            {
                WidgetProcessEventArgs e = new WidgetProcessEventArgs
                {
                    CurrentPage = CurrentPage,
                    DesignerCulture = DesignerCulture,
                    StateItem = item
                };
                if (item.WidgetInPageId > 0)
                {
                    e.WidgetInPage = CurrentPage.WidgetInPages.FirstOrDefault(it => it.Id == item.WidgetInPageId);
                }

                if (item.Widget.WidgetProcessEventListener != null)
                {
                    item.Widget.WidgetProcessEventListener.OnProcessing(e);
                }

                e.WidgetInPage = ProcessStateItem(item, item.Widget);

                if (item.Widget.WidgetProcessEventListener != null)
                {
                    item.Widget.WidgetProcessEventListener.OnProcessed(e);
                }
            }

            Database.GetCurrentSession().Commit();
        }

        private WidgetInPage ProcessStateItem(WidgetStateItem stateItem, WidgetDefinition widget)
        {
            var block = CurrentPage.Layout.FindZone(stateItem.NewZoneName);

            WidgetInPage widgetInPage = null;

            if (stateItem.State == WidgetState.Added)
            {
                widgetInPage = CurrentPage.AddWidget(block, widget, stateItem.NewOrder);
            }
            else if (stateItem.State == WidgetState.Removed)
            {
                widgetInPage = CurrentPage.WidgetInPages.FirstOrDefault(it => it.Id == stateItem.WidgetInPageId);
                CurrentPage.WidgetInPages.Remove(widgetInPage);
            }
            else if (stateItem.State == WidgetState.Changed)
            {
                widgetInPage = CurrentPage.WidgetInPages.FirstOrDefault(it => it.Id == stateItem.WidgetInPageId);
                widgetInPage.ZoneName = block.Name;
                widgetInPage.Order = stateItem.NewOrder;
            }

            if (stateItem.State != WidgetState.Removed)
            {
                foreach (var attr in stateItem.Attributes)
                {
                    widgetInPage.Attributes.AddOrSet(attr.Key, attr.Value);
                }
            }

            return widgetInPage;
        }
    }
}
