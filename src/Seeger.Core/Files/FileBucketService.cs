using Seeger.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Seeger.Files
{
    public class FileBucketService
    {
        private IFileBucketMetaStore _metaStore;

        public FileBucketService(IFileBucketMetaStore metaStore)
        {
            _metaStore = metaStore;
        }

        public FileBucketMeta CreateBucket(string displayName, string fileSystemProviderName, IDictionary<string, string> config)
        {
            var meta = new FileBucketMeta
            {
                BucketId = Guid.NewGuid().ToString("N"),
                DisplayName = displayName,
                FileSystemProviderName = fileSystemProviderName
            };

            if (config != null)
            {
                meta.Config = new Dictionary<string, string>(config);
            }

            _metaStore.Save(meta);

            return meta;
        }

        public void RemoveBucket(string bucketId)
        {
            _metaStore.Remove(bucketId);
        }

        public void SetDefault(string bucketId)
        {
            var meta = _metaStore.Load(bucketId);

            if (meta == null)
                throw new InvalidOperationException("Cannot find bucket with id \"" + bucketId + "\".");

            var currentDefault = _metaStore.LoadDefault();
            if (currentDefault != null)
            {
                currentDefault.IsDefault = false;
                _metaStore.Save(currentDefault);
            }

            meta.IsDefault = true;

            _metaStore.Save(meta);
        }
    }
}
