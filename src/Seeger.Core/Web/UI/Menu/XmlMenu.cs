using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace Seeger.Web.UI
{
    public class XmlMenu
    {
        public XmlMenuItemCollection Items { get; private set; }

        public XmlMenu()
        {
            this.Items = new XmlMenuItemCollection();
        }

        private static readonly Lazy<XmlMenu> _adminMenu = new Lazy<XmlMenu>(() =>
        {
            string path = Server.MapPath("~/App_Data/AdminMenu.xml");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Cannot find menu configuration file.", path);
            }

            return XmlMenuLoader.LoadMenu(XDocument.Load(path).Root);

        }, true);

        public static XmlMenu AdminMenu
        {
            get { return _adminMenu.Value; }
        }
    }
}
