using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Globalization;

using Seeger.Globalization;

namespace Seeger.Web.UI.Admin.Files
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Services : System.Web.Services.WebService
    {
        private CultureInfo AdminCulture
        {
            get
            {
                return AdminSession.Current.UICulture;
            }
        }

        [WebMethod]
        public OperationResult CreateFolder(string containerVirtualPath, string folderName)
        {
            if (!AdminSession.Current.User.HasPermission(null, "FileMgnt", "AddFolder"))
            {
                return CreateAccessDeniedResult();
            }

            string path = Server.MapPath(UrlUtil.Combine(containerVirtualPath, folderName));

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return OperationResult.CreateSuccessResult(ResourceFolder.Global.GetValue("FileMgnt.CreateFolderSuccess", AdminCulture));
                }

                return OperationResult.CreateErrorResult(ResourceFolder.Global.GetValue("FileMgnt.FolderAlreadyExists", AdminCulture));
            }
            catch (Exception ex)
            {
                return OperationResult.CreateErrorResult(ex);
            }
        }

        [WebMethod]
        public OperationResult Rename(bool isDirectory, string containerVirtualPath, string oldName, string newName)
        {
            if ((isDirectory && !AdminSession.Current.User.HasPermission(null, "FileMgnt", "RenameFolder"))
                || (!isDirectory && !AdminSession.Current.User.HasPermission(null, "FileMgnt", "RenameFile")))
            {
                return CreateAccessDeniedResult();
            }

            string srcPath = Server.MapPath(UrlUtil.Combine(containerVirtualPath, oldName));
            string destPath = Server.MapPath(UrlUtil.Combine(containerVirtualPath, newName));

            if (isDirectory)
            {
                if (Directory.Exists(srcPath))
                {
                    Directory.Move(srcPath, destPath);
                    return OperationResult.CreateSuccessResult(ResourceFolder.Global.GetValue("Message.OperationSuccess", AdminCulture));
                }

                return OperationResult.CreateErrorResult(ResourceFolder.Global.GetValue("FileMgnt.DirectoryNotFound", AdminCulture));
            }
            else
            {
                if (File.Exists(srcPath))
                {
                    try
                    {
                        File.Move(srcPath, destPath);
                        return OperationResult.CreateSuccessResult(ResourceFolder.Global.GetValue("Message.OperationSuccess", AdminCulture));
                    }
                    catch (Exception ex)
                    {
                        return OperationResult.CreateErrorResult(ex);
                    }
                }

                return OperationResult.CreateErrorResult(ResourceFolder.Global.GetValue("FileMgnt.FileNotFound", AdminCulture));
            }
        }

        private OperationResult CreateAccessDeniedResult()
        {
            return OperationResult.CreateErrorResult(ResourceFolder.Global.GetValue("Message.AccessDenied", AdminCulture));
        }
    }
}
