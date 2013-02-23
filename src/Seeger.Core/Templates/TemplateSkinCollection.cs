using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Web;
using Seeger.Templates;

namespace Seeger.Templates
{
    public class TemplateSkinCollection : IEnumerable<TemplateSkin>
    {
        private Template _template;
        private Dictionary<string, TemplateSkin> _skins;

        public TemplateSkinCollection(Template template)
        {
            Require.NotNull(template, "template");

            _template = template;
        }

        public int Count
        {
            get
            {
                EnsureLoaded();
                return _skins.Count;
            }
        }

        public TemplateSkin Find(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            EnsureLoaded();

            TemplateSkin theme = null;
            if (_skins.TryGetValue(name, out theme))
            {
                return theme;
            }

            return theme;
        }

        private readonly object _loadLock = new object();

        private void EnsureLoaded()
        {
            if (_skins == null)
            {
                lock (_loadLock)
                {
                    if (_skins == null)
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
                var skins = new Dictionary<string, TemplateSkin>();

                var collection = new SkinCollection(_template.SkinFolderVirtualPath);
                foreach (var skin in collection)
                {
                    skins.Add(skin.Name, new TemplateSkin(skin.Name, _template));
                }

                _skins = skins;
            }
        }

        public IEnumerator<TemplateSkin> GetEnumerator()
        {
            EnsureLoaded();
            return _skins.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
