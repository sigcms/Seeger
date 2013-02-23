using Seeger.Plugins;
using Seeger.Plugins.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Seeger.Web.UI
{
    public class WidgetStateItem
    {
        public int WidgetInPageId { get; set; }
        public WidgetDefinition Widget { get; private set; }
        public string NewZoneName { get; set; }
        public WidgetState State { get; set; }
        public int NewOrder { get; set; }
        public IList<EntityAttribute> Attributes { get; private set; }
        public object CustomData { get; set; }

        public WidgetStateItem()
        {
            this.Attributes = new List<EntityAttribute>();
        }

        public static IList<WidgetStateItem> Parse(string jsonStateItemArray)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var stateItemDics = ((object[])serializer.DeserializeObject(jsonStateItemArray)).Cast<IDictionary<string, object>>();

            return stateItemDics.Select(it => Create(it)).ToList();
        }

        public static WidgetStateItem Create(IDictionary<string, object> dic)
        {
            Require.NotNull(dic, "dic");
            Require.That(dic.ContainsKey("id"), "Require key 'id'.");
            Require.That(dic.ContainsKey("widgetName"), "Require key 'widgetName'.");
            Require.That(dic.ContainsKey("zoneName"), "Require key 'zoneName'.");
            Require.That(dic.ContainsKey("order"), "Require key 'order'.");
            Require.That(dic.ContainsKey("customData"), "Require key 'customData'.");
            Require.That(dic.ContainsKey("state"), "Require key 'state'.");

            WidgetStateItem stateItem = new WidgetStateItem();

            var plugin = PluginManager.FindEnabledPlugin(dic["pluginName"].ToString());

            string pluginName = null;
            string templateName = null;

            if (dic.ContainsKey("pluginName"))
            {
                pluginName = dic["pluginName"].ToString();
            }
            if (dic.ContainsKey("templateName"))
            {
                templateName = dic["templateName"].ToString();
            }

            string widgetName = dic["widgetName"].ToString();

            stateItem.Widget = plugin.FindWidget(widgetName);
            
            stateItem.NewZoneName = dic["zoneName"].ToString();

            int id = 0;
            int.TryParse(dic["id"].ToString(), out id);

            stateItem.WidgetInPageId = id;

            stateItem.NewOrder = Convert.ToInt32(dic["order"].ToString());
            stateItem.State = (WidgetState)Enum.Parse(typeof(WidgetState), dic["state"].ToString(), true);
            stateItem.CustomData = dic["customData"];

            if (dic.ContainsKey("attributes"))
            {
                var attributes = (IDictionary<string, object>)dic["attributes"];
                foreach (var each in attributes)
                {
                    var temp = (IDictionary<string, object>)each.Value;
                    var value = temp["value"].ToString();
                    stateItem.Attributes.Add(new EntityAttribute(each.Key, value));
                }
            }

            return stateItem;
        }
    }

}
