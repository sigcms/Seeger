using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Files;
using Seeger.Security;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Hosting;
using Seeger.Web.UI.Grid;

namespace Seeger.Web.UI.Admin.Files
{
    public partial class List : AjaxGridPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "FileMgnt", "View");
        }

        protected string CurrentPath
        {
            get
            {
                string path = Request.QueryString["path"];
                if (String.IsNullOrEmpty(path))
                {
                    path = FileExplorer.AllowedUploadPaths.First();
                }
                else if (!FileExplorer.AllowUploadPath(path))
                {
                    throw new InvalidOperationException("Path denied: " + path);
                }

                return path.TrimEnd('/');
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindToolbar();
            }
        }

        private void BindToolbar()
        {
            string[] paths = CurrentPath.SplitWithoutEmptyEntries('/');
            if (paths.Length >= 2)
            {
                UpButton.Enabled = true;
                UpButton.OnClientClick = String.Format("location.href='?path=/{0}';return false;", String.Join("/", paths, 0, paths.Length - 1));
            }

            UploadFileButton.OnClientClick = "location.href='Upload.aspx?path=" + CurrentPath + "';return false;";
        }

        [WebMethod, ScriptMethod]
        public static void Delete(string virtualPath)
        {
            var path = HostingEnvironment.MapPath(virtualPath);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}