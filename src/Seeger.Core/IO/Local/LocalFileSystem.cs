using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Seeger.IO.Local
{
    public class LocalFileSystem : IFileSystem
    {
        public IDirectory RootDirectory { get; private set; }

        public string BaseVirtualPath { get; private set; }

        public LocalFileSystem(string baseVirtualPath)
            : this(baseVirtualPath, new DirectoryInfo(HostingEnvironment.MapPath(baseVirtualPath)))
        {
        }

        public LocalFileSystem(string baseVirtualPath, DirectoryInfo rootDirectory)
        {
            BaseVirtualPath = baseVirtualPath;
            RootDirectory = new LocalDirectory(rootDirectory, this);
        }

        public IDirectory GetDirectory(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            var root = (LocalDirectory)RootDirectory;
            var path = root.FileSystemInfo.FullName;

            if (virtualPath != "/")
            {
                path = Path.Combine(path, virtualPath.Replace('/', Path.DirectorySeparatorChar));
            }

            return new LocalDirectory(new DirectoryInfo(path), this);
        }
    }
}
