using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;

using Seeger.Web;
using Seeger.Globalization;

namespace Seeger.Templates
{
    public class Skin
    {
        public string Name { get; private set; }
        public LocalizableText DisplayName { get; private set; }
        public string VirtualPath { get; private set; }
        public string PreviewImageVirtualPath
        {
            get
            {
                return UrlUtility.Combine(VirtualPath, "Preview.jpg");
            }
        }

        public Skin(string name, string virtualPath)
            : this(name, virtualPath, null)
        {
        }

        public Skin(string name, string virtualPath, string displayName)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            Name = name;
            VirtualPath = UrlUtility.ToAbsoluteHtmlPath(virtualPath);
            DisplayName = new LocalizableText(String.IsNullOrEmpty(displayName) ? "{ " + name + " }" : displayName);
        }

        public string GetFileVirtualPath(string fileName)
        {
            Require.NotNullOrEmpty(fileName, "fileName");

            return UrlUtility.Combine(VirtualPath, fileName);
        }

        public string GetFileVirtualPath(string fileName, CultureInfo culture)
        {
            Require.NotNullOrEmpty(fileName, "fileName");
            Require.NotNull(culture, "culture");

            return UrlUtility.Combine(VirtualPath, culture.Name, fileName);
        }

        public bool ContainsFile(string fileName)
        {
            Require.NotNullOrEmpty(fileName, "fileName");

            return File.Exists(Server.MapPath(UrlUtility.Combine(VirtualPath, fileName)));
        }

        public bool ContainsFile(string fileName, CultureInfo culture)
        {
            Require.NotNullOrEmpty(fileName, "fileName");
            Require.NotNull(culture, "culture");

            return File.Exists(Server.MapPath(UrlUtility.Combine(VirtualPath, culture.Name , fileName)));
        }

        public IList<string> GetCssFileVirtualPaths(CultureInfo culture)
        {
            Require.NotNull(culture, "culture");

            List<string> cssFiles = new List<string>();

            DirectoryInfo root = new DirectoryInfo(Server.MapPath(VirtualPath));
            if (root.Exists)
            {
                cssFiles.AddRange(GetCssFiles(root).Select(f => UrlUtility.Combine(VirtualPath, f.Name)));

                var cultureDir = root.GetDirectories(culture.Name);
                if (cultureDir.Length == 1)
                {
                    cssFiles.AddRange(GetCssFiles(cultureDir[0]).Select(f => UrlUtility.Combine(VirtualPath, culture.Name, f.Name)));
                }
            }

            cssFiles.Sort((x, y) => x.CompareTo(y));

            return cssFiles;
        }

        private IEnumerable<FileInfo> GetCssFiles(DirectoryInfo directory)
        {
            return from f in directory.GetFiles("*.css")
                   where !f.IsHidden()
                   orderby f.Name
                   select f;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
