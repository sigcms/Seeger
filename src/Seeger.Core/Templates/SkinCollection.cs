using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Seeger.Web;

namespace Seeger.Templates
{
    public class SkinCollection : IEnumerable<Skin>
    {
        private string _containerVirtualPath;

        public SkinCollection(string containerVirtualPath)
        {
            Require.NotNullOrEmpty(containerVirtualPath, "containerVirtualPath");

            _containerVirtualPath = containerVirtualPath;
        }

        public int Count
        {
            get
            {
                EnsureLoaded();
                return _themes.Count;
            }
        }

        private readonly object _loadLock = new object();

        private void EnsureLoaded()
        {
            if (_themes == null)
            {
                lock (_loadLock)
                {
                    if (_themes == null)
                    {
                        Refresh();
                    }
                }
            }
        }

        private readonly object _reloadLock = new object();

        public void Refresh()
        {
            lock (_reloadLock)
            {
                var themes = new Dictionary<string, Skin>(StringComparer.OrdinalIgnoreCase);
                var dir = new DirectoryInfo(Server.MapPath(_containerVirtualPath));

                if (dir.Exists)
                {
                    foreach (var each in dir.GetDirectories())
                    {
                        if (!each.IsHidden())
                        {
                            themes.Add(each.Name, new Skin(each.Name, UrlUtility.Combine(_containerVirtualPath, each.Name)));
                        }
                    }
                }

                _themes = themes;
            }
        }

        private Dictionary<string, Skin> _themes;

        public Skin FindTheme(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            EnsureLoaded();

            Skin theme;
            _themes.TryGetValue(name, out theme);

            return theme;
        }

        public IEnumerator<Skin> GetEnumerator()
        {
            EnsureLoaded();
            return _themes.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
