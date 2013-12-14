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
        public IEnumerable<FileSystemEntryInfo> List(string path, string filter)
        {
            //if (!FileExplorer.AllowUploadPath(path))
            //    throw new InvalidOperationException("Path '" + path + "' is not allowed.");

            var extensions = String.IsNullOrEmpty(filter) || filter == "*.*" ? null : new HashSet<string>(filter.SplitWithoutEmptyEntries(';'), StringComparer.OrdinalIgnoreCase);

            throw new NotImplementedException();
            //var fileSystem = FileSystems.Current;
            //var directory = fileSystem.GetDirectory(path);
            //var entries = new List<FileSystemEntryInfo>();

            //entries.AddRange(directory.GetDirectories().Select(x => new FileSystemEntryInfo(x)));

            //var files = directory.GetFiles();

            //if (extensions != null)
            //{
            //    files = files.Where(x => extensions.Contains(x.Extension));
            //}

            //entries.AddRange(files.Select(x => new FileSystemEntryInfo(x)));

            //return entries;
        }

        [WebMethod]
        public void CreateFolder(string path, string folderName)
        {
            //if (!FileExplorer.AllowUploadPath(path))
            //    throw new InvalidOperationException("Path '" + path + "' is not allowed.");

            if (!AdminSession.Current.User.HasPermission(null, "FileMgnt", "AddFolder"))
                throw new InvalidOperationException(ResourceFolder.Global.GetValue("Message.AccessDefined"));

            throw new NotImplementedException();
            //var directory = FileSystems.Current.GetDirectory(path);
            //directory.CreateFolder(folderName);
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
