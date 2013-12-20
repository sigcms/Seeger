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
    public class QiniuDirectory : IDirectory
    {
        public QiniuFileSystem FileSystem { get; private set; }

        public IFileSystemIndex Index
        {
            get
            {
                return FileSystem.Index;
            }
        }

        public string VirtualPath { get; private set; }

        public string PublicUri
        {
            get
            {
                return FileSystem.GetPublicUri(VirtualPath);
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileName(VirtualPath);
            }
        }

        public string Extension
        {
            get
            {
                return String.Empty;
            }
        }

        public bool Exists
        {
            get
            {
                return true;
            }
        }

        public QiniuDirectory(string virtualPath, QiniuFileSystem fileSystem)
        {
            VirtualPath = virtualPath;
            FileSystem = fileSystem;
        }

        public void Create()
        {
            Index.AddDirectory(VirtualPath);
        }

        public IDirectory CreateSubdirectory(string folderName)
        {
            var directory = new QiniuDirectory(UrlUtil.Combine(VirtualPath, folderName), FileSystem);
            directory.Create();
            return directory;
        }

        public IDirectory GetDirectory(string folderName)
        {
            var directoryIndex = Index.GetDirectories(VirtualPath).FirstOrDefault(x => x.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase));
            if (directoryIndex != null)
            {
                return new QiniuDirectory(UrlUtil.Combine(VirtualPath, folderName), FileSystem);
            }

            return null;
        }

        public IEnumerable<IDirectory> GetDirectories()
        {
            foreach (var index in Index.GetDirectories(VirtualPath))
            {
                yield return new QiniuDirectory(UrlUtil.Combine(VirtualPath, index.Name), FileSystem);
            }
        }

        public IFile CreateFile(string fileName)
        {
            var indexEntry = new FileIndexEntry
            {
                Name = fileName,
                CreationTimeUtc = DateTime.UtcNow,
                LastWriteTimeUtc = DateTime.UtcNow
            };

            Index.AddFile(VirtualPath, indexEntry);

            return new QiniuFile(UrlUtil.Combine(VirtualPath, fileName), indexEntry, FileSystem);
        }

        public IFile GetFile(string fileName)
        {
            var fileIndex = Index.GetFiles(VirtualPath).FirstOrDefault(x => x.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase));
            if (fileIndex != null)
            {
                return new QiniuFile(UrlUtil.Combine(VirtualPath, fileName), fileIndex, FileSystem);
            }

            return null;
        }

        public IEnumerable<IFile> GetFiles()
        {
            foreach (var index in Index.GetFiles(VirtualPath))
            {
                yield return new QiniuFile(UrlUtil.Combine(VirtualPath, index.Name), index, FileSystem);
            }
        }
    }
}