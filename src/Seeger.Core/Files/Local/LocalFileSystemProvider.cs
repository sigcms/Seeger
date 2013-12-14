using System;
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

        public IFileSystem CreateFileSystem(System.Collections.Specialized.NameValueCollection config)
        {
            var baseVirtualPath = config["BaseVirtualPath"];

            if (String.IsNullOrEmpty(baseVirtualPath))
                throw new InvalidOperationException("Missing configuration parameter \"BaseVirtualPath\".");

            return new LocalFileSystem(baseVirtualPath);
        }
    }
}
