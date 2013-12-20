using Qiniu.RS;
using Seeger.Files;
using Seeger.Files.Indexing;
using Seeger.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Plugins.QiniuCloud
{
    public class QiniuFile : IFile
    {
        private FileIndexEntry _indexEntry;

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
                return Path.GetExtension(VirtualPath);
            }
        }

        public bool Exists
        {
            get
            {
                return true;
            }
        }

        public long Length
        {
            get
            {
                return _indexEntry.Length;
            }
        }

        public DateTime CreationTimeUtc
        {
            get
            {
                return _indexEntry.CreationTimeUtc;
            }
        }

        public DateTime LastWriteTimeUtc
        {
            get
            {
                return _indexEntry.LastWriteTimeUtc;
            }
        }

        public QiniuFile(string virtualPath, FileIndexEntry indexEntry, QiniuFileSystem fileSystem)
        {
            VirtualPath = virtualPath;
            FileSystem = fileSystem;
            _indexEntry = indexEntry;
        }

        public System.IO.Stream Read()
        {
            throw new NotImplementedException();
        }

        public void Write(System.IO.Stream stream)
        {
            var length = stream.Length;

            var client = new Qiniu.IO.IOClient();
            var putPolicy = new PutPolicy(FileSystem.Settings.Bucket);
            var token = putPolicy.Token(new Qiniu.Auth.digest.Mac(FileSystem.Settings.AccessKey, Encoding.UTF8.GetBytes(FileSystem.Settings.SecurityKey)));
            var result = client.Put(token, VirtualPath.TrimStart('/'), stream, null);

            if (!result.OK)
                throw new QiniuException(result.StatusCode + ", " + result.Response, result.Exception);

            _indexEntry.Length = length;
            _indexEntry.LastWriteTimeUtc = DateTime.UtcNow;

            Index.UpdateFile(UrlUtil.GetParentPath(VirtualPath), _indexEntry);
        }
    }
}