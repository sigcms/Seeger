using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public interface IFileBucketMetaStore
    {
        int GetBucketCount();

        FileBucketMeta Load(string bucketId);

        FileBucketMeta LoadDefault();

        IList<FileBucketMeta> LoadAll();

        void Save(FileBucketMeta meta);

        void Remove(string bucketId);
    }

    public static class FileBucketMetaStores
    {
        public static IFileBucketMetaStore Current = new XmlFileBucketMetaStore(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Buckets"));
    }
}
