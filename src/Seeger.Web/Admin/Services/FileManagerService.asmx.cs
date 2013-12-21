using Seeger.Files;
using Seeger.Globalization;
using Seeger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Seeger.Web.UI.Admin.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class FileManagerService : System.Web.Services.WebService
    {
        [WebMethod]
        public object Buckets()
        {
            return FileBucketMetaStores.Current.LoadAll()
                                       .Select(x => new
                                       {
                                           BucketId = x.BucketId,
                                           DisplayName = x.DisplayName,
                                           IsDefault = x.IsDefault
                                       });
        }

        [WebMethod]
        public IEnumerable<FileSystemEntryInfo> List(string bucketId, string path, string filter)
        {
            var extensions = String.IsNullOrEmpty(filter) || filter == "*.*" ? null : new HashSet<string>(filter.SplitWithoutEmptyEntries(';'), StringComparer.OrdinalIgnoreCase);

            var fileSystem = LoadFileSystem(bucketId);

            var directory = fileSystem.GetDirectory(path);
            var entries = new List<FileSystemEntryInfo>();

            entries.AddRange(directory.GetDirectories().Select(x => new FileSystemEntryInfo(x)));

            var files = directory.GetFiles();

            if (extensions != null)
            {
                files = files.Where(x => extensions.Contains(x.Extension));
            }

            entries.AddRange(files.Select(x => new FileSystemEntryInfo(x)));

            return entries;
        }

        [WebMethod]
        public void CreateFolder(string bucketId, string path, string folderName)
        {
            if (!AdminSession.Current.User.HasPermission(null, "FileMgnt", "AddFolder"))
                throw new InvalidOperationException(ResourceFolder.Global.GetValue("Message.AccessDefined"));

            var fileSystem = LoadFileSystem(bucketId);
            var directory = fileSystem.GetDirectory(path);
            directory.CreateSubdirectory(folderName);
        }

        private IFileSystem LoadFileSystem(string bucketId)
        {
            FileBucketMeta meta = null;

            if (!String.IsNullOrWhiteSpace(bucketId))
            {
                meta = FileBucketMetaStores.Current.Load(bucketId);
            }
            else
            {
                meta = FileBucketMetaStores.Current.LoadDefault();
            }

            var provider = FileSystemProviders.Get(meta.FileSystemProviderName);
            return provider.LoadFileSystem(meta);
        }
    }

    public class FileSystemEntryInfo
    {
        public string Name { get; set; }

        public string VirtualPath { get; set; }

        public string PublicUri { get; set; }

        public bool IsDirectory { get; set; }

        public long Length { get; set; }

        public FileSystemEntryInfo()
        {
        }

        public FileSystemEntryInfo(IFileSystemEntry entry)
        {
            Name = entry.Name;
            VirtualPath = entry.VirtualPath;
            PublicUri = entry.PublicUri;
            IsDirectory = entry is IDirectory;

            if (!IsDirectory)
            {
                Length = ((IFile)entry).Length;
            }
        }
    }
}
