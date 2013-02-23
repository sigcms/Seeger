using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Globalization;
using System.Xml.Linq;
using Seeger.Plugins;

namespace Seeger.Security
{
    public class PermissionGroup
    {
        public string Name { get; private set; }

        public LocalizableText DisplayName { get; private set; }

        public PluginDefinition Plugin { get; private set; }

        public PermissionCollection Permissions { get; private set; }

        public PermissionGroup(string name)
            : this(name, name, null)
        {
        }

        public PermissionGroup(string name, string displayName)
            : this(name, displayName, null)
        {
        }

        public PermissionGroup(string name, PluginDefinition plugin)
            : this(name, name, plugin)
        {
        }

        public PermissionGroup(string name, string displayName, PluginDefinition plugin)
        {
            Require.NotNullOrEmpty(name, "name");

            this.Name = name;
            this.DisplayName = new LocalizableText(displayName);
            this.Plugin = plugin;
            this.Permissions = new PermissionCollection();

            if (plugin != null && String.IsNullOrEmpty(DisplayName.ResourceDescriptor.PluginName))
            {
                DisplayName.ResourceDescriptor.PluginName = plugin.Name;
            }
        }

        public static PermissionGroup From(XElement xml, PluginDefinition plugin)
        {
            var group = new PermissionGroup(xml.AttributeValue("name"), xml.AttributeValue("display-name"), plugin);
            foreach (var child in xml.Elements())
            {
                group.Permissions.Add(Permission.From(child, group));
            }

            return group;
        }

        public override string ToString()
        {
            return Name + ": " + String.Join<string>(",", Permissions.Select(it => it.Name));
        }
    }

    public class Permission : IComparable<Permission>
    {
        public string Name { get; private set; }

        public LocalizableText DisplayName { get; private set; }

        public int Weight { get; private set; }

        public PermissionGroup Group { get; set; }

        public PluginDefinition Plugin
        {
            get
            {
                return Group == null ? null : Group.Plugin;
            }
        }

        public Permission(string name, int weight, PermissionGroup group)
            : this(name, weight, group, null)
        {
        }

        public Permission(string name, int weight, PermissionGroup group, string displayName)
        {
            Require.NotNullOrEmpty(name, "name");

            this.Name = name;
            this.Weight = weight < 0 ? 0 : weight;
            this.Group = group;
            this.DisplayName = new LocalizableText(displayName);

            if (group.Plugin != null)
            {
                DisplayName.ResourceDescriptor.PluginName = group.Plugin.Name;
            }
        }

        public static Permission From(XElement xml, PermissionGroup group)
        {
            return new Permission(xml.AttributeValue("name"), xml.AttributeValue<int>("weight"), group, xml.AttributeValue("display-name"));
        }

        public override string ToString()
        {
            return Name + "(" + Weight + ")";
        }

        public int CompareTo(Permission other)
        {
            if (other == null)
            {
                return 1;
            }

            return Weight.CompareTo(other.Weight);
        }
    }
}
