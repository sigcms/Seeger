using Seeger.Files;
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
        public IEnumerable<FileSystemEntry> List(string path)
        {
            if (!FileExplorer.AllowUploadPath(path))
                throw new InvalidOperationException("Path '" + path + "' is not allowed.");

            return FileExplorer.List(path);
        }
    }
}
