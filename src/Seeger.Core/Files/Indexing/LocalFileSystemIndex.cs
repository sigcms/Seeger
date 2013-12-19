using Newtonsoft.Json;
using Seeger.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files.Indexing
{
    public class LocalFileSystemIndex : IFileSystemIndex
    {
        public string Path { get; private set; }

        public LocalFileSystemIndex(string path)
        {
            Require.NotNullOrEmpty(path, "path");
            Path = path;
        }

        public IEnumerable<FileIndexEntry> GetFiles(string virtualPath)
        {
            var metaPath = GetPhysicalPath(UrlUtil.Combine(virtualPath, "meta.config"));
            return new DirectoryMetaFile(metaPath).ReadEntries();
        }

        public void AddFile(string directoryVirtualPath, FileIndexEntry file)
        {
            var directoryPath = GetPhysicalPath(directoryVirtualPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var metaPath = System.IO.Path.Combine(directoryPath, "meta.config");
            var metaFile = new DirectoryMetaFile(metaPath);
            metaFile.AddEntry(file);
        }

        public void DeleteFile(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            if (virtualPath == "/")
                throw new ArgumentException("Invalid virtual path.", "virtualPath");

            var filePath = GetPhysicalPath(virtualPath);
            var dirPath = System.IO.Path.GetDirectoryName(filePath);

            var metaPath = System.IO.Path.Combine(dirPath, "meta.config");

            if (!File.Exists(metaPath))
                throw new InvalidOperationException("File not exists.");

            var metaFile = new DirectoryMetaFile(metaPath);
            metaFile.RemoveEntry(System.IO.Path.GetFileName(virtualPath));
        }

        public IEnumerable<DirectoryIndexEntry> GetDirectories(string virtualPath)
        {
            var directory = new DirectoryInfo(GetPhysicalPath(virtualPath));
            if (directory.Exists)
            {
                foreach (var dir in directory.EnumerateDirectories())
                {
                    yield return new DirectoryIndexEntry
                    {
                        Name = dir.Name,
                        Extension = String.Empty
                    };
                }
            }
        }

        public void AddDirectory(string virtualPath)
        {
            var path = GetPhysicalPath(virtualPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void DeleteDirectory(string virtualPath)
        {
            var path = GetPhysicalPath(virtualPath);

            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }

        public string GetPhysicalPath(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            if (virtualPath == "/")
            {
                return Path;
            }

            return System.IO.Path.Combine(Path, virtualPath.Replace('/', '\\'));
        }
    }
}
