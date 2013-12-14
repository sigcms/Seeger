using Newtonsoft.Json;
using Seeger.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public class XmlFileBucketMetaStore : IFileBucketMetaStore
    {
        private Lazy<ConcurrentDictionary<string, FileBucketMeta>> _cache;

        private ConcurrentDictionary<string, FileBucketMeta> Cache
        {
            get
            {
                return _cache.Value;
            }
        }

        public string ContainerDirectory { get; private set; }

        public XmlFileBucketMetaStore(string containerDirectory)
        {
            Require.NotNullOrEmpty(containerDirectory, "containerDirectory");
            ContainerDirectory = containerDirectory;
            _cache = new Lazy<ConcurrentDictionary<string, FileBucketMeta>>(LoadFromDisk, true);
        }

        public int GetBucketCount()
        {
            return Cache.Count;
        }

        public FileBucketMeta Load(string bucketId)
        {
            FileBucketMeta meta;

            if (Cache.TryGetValue(bucketId, out meta))
            {
                return meta.Clone();
            }

            return null;
        }

        public FileBucketMeta LoadDefault()
        {
            var defaultMeta = Cache.Values.FirstOrDefault(x => x.IsDefault);
            return defaultMeta == null ? null : defaultMeta.Clone();
        }

        public IList<FileBucketMeta> LoadAll()
        {
            return Cache.Values.Select(x => x.Clone()).ToList();
        }

        public void Save(FileBucketMeta meta)
        {
            Require.NotNull(meta, "meta");

            if (!Directory.Exists(ContainerDirectory))
            {
                Directory.CreateDirectory(ContainerDirectory);
            }

            var path = GetBucketMetaFilePath(meta.BucketId);
            File.WriteAllText(path, JsonConvert.SerializeObject(meta));

            FileBucketMeta cachedMeta;
            Cache.TryRemove(meta.BucketId, out cachedMeta);
            Cache.TryAdd(meta.BucketId, meta.Clone());
        }

        public void Remove(string bucketId)
        {
            FileBucketMeta meta = null;

            if (!Cache.TryGetValue(bucketId, out meta))
                throw new InvalidOperationException("Bucket with id \"" + bucketId + "\" does not exist.");

            // TODO: The better solution is to use some tree structure,
            //       then we can use copy on write to replace the node to achieve transactional deletion
            File.Delete(GetBucketMetaFilePath(bucketId));
            Cache.TryRemove(bucketId, out meta);
        }

        private string GetBucketMetaFilePath(string bucketId)
        {
            return Path.Combine(ContainerDirectory, bucketId + ".bucket");
        }

        private ConcurrentDictionary<string, FileBucketMeta> LoadFromDisk()
        {
            var metasById = new ConcurrentDictionary<string, FileBucketMeta>();

            if (Directory.Exists(ContainerDirectory))
            {
                foreach (var file in Directory.EnumerateFiles(ContainerDirectory, "*.bucket", SearchOption.TopDirectoryOnly))
                {
                    var meta = JsonConvert.DeserializeObject<FileBucketMeta>(File.ReadAllText(file, Encoding.UTF8));
                    metasById.TryAdd(Path.GetFileNameWithoutExtension(file), meta);
                }
            }

            return metasById;
        }
    }
}
