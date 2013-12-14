using Seeger.Files.Local;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public static class FileSystems
    {
        static readonly ConcurrentDictionary<string, IFileSystem> _fileSystems = new ConcurrentDictionary<string, IFileSystem>();

        static FileSystems()
        {
        }

        public static IFileSystem GetFileSystem(string bucketId)
        {
            IFileSystem fileSystem = null;

            if (_fileSystems.TryGetValue(bucketId, out fileSystem))
            {
                return fileSystem;
            }

            return null;
        }

        public static bool Remove(string bucketId)
        {
            IFileSystem fileSystem;
            return _fileSystems.TryRemove(bucketId, out fileSystem);
        }

        public static void Register(string bucketId, IFileSystem fileSystem)
        {
            Require.NotNull(fileSystem, "fileSystem");

            var success = _fileSystems.TryAdd(bucketId, fileSystem);

            if (!success)
                throw new InvalidOperationException("Cannot register bucket id: " + bucketId + ".");
        }
    }
}
