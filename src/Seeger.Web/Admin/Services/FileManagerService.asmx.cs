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
        public IEnumerable<FileSystemEntryInfo> List(string path)
        {
            if (!FileExplorer.AllowUploadPath(path))
                throw new InvalidOperationException("Path '" + path + "' is not allowed.");

            return FileExplorer.List(path).Select(x => new FileSystemEntryInfo(x));
        }

        [WebMethod]
        public void CreateFolder(string path, string folderName)
        {
            if (!FileExplorer.AllowUploadPath(path))
                throw new InvalidOperationException("Path '" + path + "' is not allowed.");

            if (!AdminSession.Current.User.HasPermission(null, "FileMgnt", "AddFolder"))
                throw new InvalidOperationException(ResourceFolder.Global.GetValue("Message.AccessDefined"));

            IOUtil.EnsureDirectoryCreated(Server.MapPath(UrlUtil.Combine(path, folderName)));
        }
    }

    public class FileSystemEntryInfo
    {
        public string Name { get; set; }

        public string VirtualPath { get; set; }

        public bool IsDirectory { get; set; }

        public long Length { get; set; }

        public FileSystemEntryInfo()
        {
        }

        public FileSystemEntryInfo(FileSystemEntry entry)
        {
            Name = entry.Name;
            VirtualPath = entry.VirtualPath;
            IsDirectory = entry.IsDirectory;
            Length = entry.Length;
        }
    }
}
