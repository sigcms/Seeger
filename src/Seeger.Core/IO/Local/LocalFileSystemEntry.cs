using Seeger.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.IO.Local
{
    public abstract class LocalFileSystemEntry : IFileSystemEntry
    {
        public LocalFileSystem FileSystem { get; private set; }

        public FileSystemInfo FileSystemInfo { get; private set; }

        public string VirtualPath
        {
            get
            {
                var rootDirectory = (LocalDirectory)FileSystem.RootDirectory;

                if (FileSystemInfo.FullName.Length == rootDirectory.FileSystemInfo.FullName.Length)
                {
                    return "/";
                }

                return FileSystemInfo.FullName.Substring(rootDirectory.FileSystemInfo.FullName.Length).Replace(Path.PathSeparator, '/');
            }
        }

        public string PublicUri
        {
            get
            {
                return UrlUtil.Combine(FileSystem.BaseVirtualPath, VirtualPath);
            }
        }

        public string Name
        {
            get
            {
                return FileSystemInfo.Name;
            }
        }

        public string Extension
        {
            get
            {
                return FileSystemInfo.Extension;
            }
        }

        public bool Exists
        {
            get
            {
                return FileSystemInfo.Exists;
            }
        }

        protected LocalFileSystemEntry(FileSystemInfo fileSystemInfo, LocalFileSystem fileSystem)
        {
            FileSystemInfo = fileSystemInfo;
            FileSystem = fileSystem;
        }

        public override string ToString()
        {
            return VirtualPath;
        }
    }
}
