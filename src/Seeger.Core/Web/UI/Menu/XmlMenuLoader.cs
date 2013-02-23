using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Seeger.Web.UI
{
    public class XmlMenuLoader
    {
        public static XmlMenu LoadMenu(XElement container)
        {
            Require.NotNull(container, "parent");

            return LoadMenu(container.Elements());
        }

        public static XmlMenu LoadMenu(IEnumerable<XElement> menuItemElements)
        {
            Require.NotNull(menuItemElements, "rootMenuElements");

            XmlMenu menu = new XmlMenu();

            foreach (var element in menuItemElements)
            {
                XmlMenuItem item = LoadMenuItem(element);
                menu.Items.Add(item);
            }

            return menu;
        }

        static XmlMenuItem LoadMenuItem(XElement menuElement)
        {
            Require.That(IsValidMenuElement(menuElement), "'rootMenuElement' should be a valid menu element.");

            XmlMenuItem root = XmlMenuItem.FromXml(menuElement);
            LoadChildren(root, menuElement);

            return root;
        }

        static void LoadChildren(XmlMenuItem parent, XElement parentElement)
        {
            foreach (var child in parentElement.Elements())
            {
                if (IsValidMenuElement(child))
                {
                    XmlMenuItem item = XmlMenuItem.FromXml(child);
                    parent.Items.Add(item);
                    LoadChildren(item, child);
                }
            }
        }

        static bool IsValidMenuElement(XElement element)
        {
            return element.Name == "menu";
        }
    }
}
