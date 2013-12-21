using Seeger.Files;
using Seeger.Files.Indexing;
using Seeger.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.QiniuCloud
{
    public class QiniuFileSystem : IFileSystem
    {
        public IFileSystemIndex Index { get; private set; }

        public QiniuBucketSettings Settings { get; private set; }

        public IDirectory RootDirectory { get; private set; }

        public QiniuFileSystem(QiniuBucketSettings bucketSettings)
            : this(bucketSettings, new LocalFileSystemIndex(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Qiniu", bucketSettings.Bucket)))
        {
        }

        public QiniuFileSystem(QiniuBucketSettings bucketSettings, IFileSystemIndex index)
        {
            Settings = bucketSettings;
            RootDirectory = new QiniuDirectory("/", this);
            Index = index;
        }

        public IDirectory GetDirectory(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            if (virtualPath == "/")
            {
                return RootDirectory;
            }

            virtualPath = virtualPath.TrimEnd('/');

            var root = (QiniuDirectory)RootDirectory;
            var folderName = Path.GetFileName(virtualPath);
            var indexEntry = Index.GetDirectories(UrlUtil.GetParentPath(virtualPath))
                                  .FirstOrDefault(x => x.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase));

            if (indexEntry != null)
            {
                return new QiniuDirectory(virtualPath, this);
            }

            return null;
        }

        public string GetPublicUri(string virtualPath)
        {
            return UrlUtil.Combine("http://" + Settings.Domain, virtualPath);
        }
    }
}