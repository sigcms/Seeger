using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Globalization;
using Seeger.Plugins;
using Seeger.Plugins.Widgets;

namespace Seeger.Web.UI.Admin.Designer.Controls
{
    public partial class AllWidgets : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        private PageDesignerBase DesignerPage
        {
            get
            {
                return (PageDesignerBase)Page;
            }
        }

        protected string PageTemplateName
        {
            get
            {
                return DesignerPage.PageItem.Layout.Template.Name;
            }
        }

        private void Bind()
        {
            var plugins = PluginManager.EnabledPlugins.Where(x => x.Widgets.Count > 0).ToList();

            var currentCategoryKey = 1;

            WidgetCategory uncategorized = null;

            var categories = new List<WidgetCategory>();

            foreach (var widget in plugins.SelectMany(x => x.Widgets))
            {
                if (widget.Category == null)
                {
                    if (uncategorized == null)
                    {
                        uncategorized = new WidgetCategory
                        {
                            Key = currentCategoryKey++,
                            Name = Localize("Common.Uncategorized")
                        };
                    }

                    uncategorized.Widgets.Add(widget);
                }
                else
                {
                    var categoryName = widget.Category.Localize(AdminSession.Current.UICulture);
                    var category = categories.FirstOrDefault(x => x.Name.IgnoreCaseEquals(categoryName));
                    if (category == null)
                    {
                        category = new WidgetCategory
                        {
                            Key = currentCategoryKey++,
                            Name = categoryName
                        };
                        categories.Add(category);
                    }

                    category.Widgets.Add(widget);
                }
            }

            categories.Sort((x, y) => x.Name.CompareTo(y.Name));

            if (uncategorized != null)
            {
                categories.Insert(0, uncategorized);
            }

            CategoryRepeater.DataSource = categories;
            CategoryRepeater.DataBind();

            CategoryWidgetsRepeater.DataSource = categories;
            CategoryWidgetsRepeater.DataBind();
        }

        protected void CategoryWidgetsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsDataItem())
            {
                var cagtegory = (WidgetCategory)e.Item.DataItem;
                var widgetList = (WidgetList)e.Item.FindControl("WidgetList");
                widgetList.Bind(cagtegory.Widgets);
            }
        }

        protected string Localize(string key)
        {
            return ResourcesFolder.Global.GetValue(key, CultureInfo.CurrentUICulture);
        }

        public class WidgetCategory
        {
            public int Key { get; set; }

            public string Name { get; set; }

            public IList<WidgetDefinition> Widgets { get; set; }

            public WidgetCategory()
            {
                Widgets = new List<WidgetDefinition>();
            }
        }
    }
}