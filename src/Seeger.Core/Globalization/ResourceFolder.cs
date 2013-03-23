using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Globalization
{
    public class ResourceFolder
    {
        public static readonly ResourceFolder Global = new ResourceFolder(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Resources"));

        public string Path { get; private set; }

        private Dictionary<CultureInfo, Dictionary<string, string>> _cache = new Dictionary<CultureInfo, Dictionary<string, string>>();

        public IEnumerable<CultureInfo> Cultures
        {
            get
            {
                return _cache.Keys;
            }
        }

        public ResourceFolder(string path)
        {
            Path = path;
            Reload();
        }

        public void Reload()
        {
            lock (_cache)
            {
                var cache = new Dictionary<CultureInfo, Dictionary<string, string>>();
                var directory = new DirectoryInfo(Path);
                if (directory.Exists)
                {
                    foreach (var subdir in directory.GetDirectories())
                    {
                        if (subdir.IsHidden()) continue;

                        var items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        foreach (var file in subdir.GetFiles("*.xml"))
                        {
                            if (file.IsHidden()) continue;

                            foreach (var entry in XmlResourceReader.ReadFrom(file.FullName))
                            {
                                items.Add(entry.Key, entry.Value);
                            }
                        }

                        var culture = CultureInfo.GetCultureInfo(subdir.Name);
                        cache.Add(culture, items);
                    }
                }

                _cache = cache;
            }
        }

        public string GetValue(string key)
        {
            return GetValue(key, CultureInfo.CurrentUICulture);
        }

        public string GetValue(string key, CultureInfo culture)
        {
            Dictionary<string, string> items;
            if (_cache.TryGetValue(culture, out items))
            {
                string value;
                items.TryGetValue(key, out value);
                return value;
            }

            return null;
        }

        public IDictionary<string, string> GetResourceDictionary(CultureInfo culture)
        {
            Dictionary<string, string> dictionary;
            _cache.TryGetValue(culture, out dictionary);
            return dictionary;
        }
    }
}
