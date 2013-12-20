using Seeger.Files.Local;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public static class FileSystemProviders
    {
        static readonly ConcurrentDictionary<string, IFileSystemProvider> _providers = new ConcurrentDictionary<string, IFileSystemProvider>();

        public static IEnumerable<IFileSystemProvider> Providers
        {
            get
            {
                return _providers.Values;
            }
        }

        static FileSystemProviders()
        {
            Register(new LocalFileSystemProvider());
        }

        public static IFileSystemProvider Get(string name)
        {
            return _providers[name];
        }

        public static void Register(IFileSystemProvider provider)
        {
            _providers.TryAdd(provider.Name, provider);
        }

        public static void Unregister(string name)
        {
            IFileSystemProvider provider;
            _providers.TryRemove(name, out provider);
        }
    }
}
