using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Files.Local
{
    public class LocalFileSystemProvider : IFileSystemProvider
    {
        public string Name
        {
            get
            {
                return "Local";
            }
        }

        public string GetConfigurationUrl(string bucketId)
        {
            return "/Admin/Files/LocalFileSystemConfig.aspx?bucketId=" + bucketId;
        }

        private ConcurrentDictionary<string, IFileSystem> _fileSystemsCache = new ConcurrentDictionary<string, IFileSystem>();

        public IFileSystem LoadFileSystem(FileBucketMeta meta)
        {
            IFileSystem fileSystem;

            if (!_fileSystemsCache.TryGetValue(meta.BucketId, out fileSystem))
            {
                fileSystem = new LocalFileSystem(meta.Config["BaseVirtualPath"]);
                _fileSystemsCache.TryAdd(meta.BucketId, fileSystem);
            }

            return fileSystem;
        }
    }
}
