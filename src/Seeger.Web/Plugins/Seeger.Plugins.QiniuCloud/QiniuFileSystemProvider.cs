using Seeger.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.QiniuCloud
{
    public class QiniuFileSystemProvider : IFileSystemProvider
    {
        public string Name
        {
            get
            {
                return Strings.FileSystemProviderName;
            }
        }

        public string GetConfigurationUrl(string bucketId)
        {
            return "/Plugins/" + Strings.PluginName + "/Admin/Settings.aspx?bucketId=" + bucketId;
        }

        public IFileSystem LoadFileSystem(FileBucketMeta meta)
        {
            var settings = new QiniuBucketSettings
            {
                Bucket = meta.Config["Bucket"],
                Domain = meta.Config["Domain"],
                AccessKey = meta.Config["AccessKey"],
                SecurityKey = meta.Config["SecurityKey"]
            };

            return new QiniuFileSystem(settings);
        }
    }
}