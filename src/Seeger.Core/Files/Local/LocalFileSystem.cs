using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Seeger.Files.Local
{
    public class LocalFileSystem : IFileSystem
    {
        public IDirectory RootDirectory { get; private set; }

        public string BaseVirtualPath { get; private set; }

        public LocalFileSystem(string baseVirtualPath)
        {
            BaseVirtualPath = baseVirtualPath;
            RootDirectory = new LocalDirectory(new DirectoryInfo(HostingEnvironment.MapPath(BaseVirtualPath)), this);
        }

        public IDirectory GetDirectory(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            var path = GetPhysicalPath(virtualPath);
            return new LocalDirectory(new DirectoryInfo(path), this);
        }

        public IDirectory CreateDirectory(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            var path = GetPhysicalPath(virtualPath);
            var directory = new LocalDirectory(new DirectoryInfo(path), this);
            directory.Create();

            return directory;
        }

        private string GetPhysicalPath(string virtualPath)
        {
            var root = (LocalDirectory)RootDirectory;
            var path = root.FileSystemInfo.FullName;

            if (virtualPath != "/")
            {
                path = Path.Combine(path, virtualPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            }
            return path;
        }
    }
}
