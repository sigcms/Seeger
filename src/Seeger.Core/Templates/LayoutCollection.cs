using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Seeger.Web;

namespace Seeger.Templates
{
    public class LayoutCollection : IEnumerable<Layout>
    {
        private Template _template;
        private Dictionary<string, Layout> _layouts;

        public LayoutCollection(Template template)
        {
            Require.NotNull(template, "template");

            _template = template;
        }

        public Layout FindLayout(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            EnsureLoaded();

            Layout layout = null;
            if (_layouts.TryGetValue(name, out layout))
            {
                return layout;
            }

            return null;
        }

        public int Count
        {
            get
            {
                EnsureLoaded();
                return _layouts.Count;
            }
        }

        private readonly object _loadLock = new object();

        private void EnsureLoaded()
        {
            if (_layouts == null)
            {
                lock (_loadLock)
                {
                    if (_layouts == null)
                    {
                        Refresh();
                    }
                }
            }
        }

        private readonly object _refreshLock = new object();

        public void Refresh()
        {
            lock (_refreshLock)
            {
                var layouts = new Dictionary<string, Layout>();

                string path = Server.MapPath(UrlUtil.Combine(_template.VirtualPath, "Layouts"));

                DirectoryInfo directory = new DirectoryInfo(path);
                if (directory.Exists)
                {
                    foreach (var dir in directory.GetDirectories())
                    {
                        if (!dir.IsHidden())
                        {
                            var layout = new Layout(dir.Name, _template);
                            layouts.Add(layout.Name, layout);
                        }
                    }
                }

                _layouts = layouts;
            }
        }

        public IEnumerator<Layout> GetEnumerator()
        {
            EnsureLoaded();
            return _layouts.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
