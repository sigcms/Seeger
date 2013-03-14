using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Files;
using Seeger.Security;
using Seeger.Utils;

namespace Seeger.Web.UI.Admin.Files
{
    public partial class Upload : AdminPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "FileMgnt", "UploadFile");
        }

        protected string Path
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
                    throw new InvalidOperationException("Upload path denied: " + path);
                }

                return path;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                string fileName = FileExplorer.ApplySecurityFilterToFileName(System.IO.Path.GetFileName(FileUpload.FileName));
                string ext = System.IO.Path.GetExtension(fileName);

                if (FileExplorer.SupportFileExtension(ext))
                {
                    fileName = FileExplorer.CalculateFinalName(Path, fileName);

                    IOUtil.EnsureDirectoryCreated(Server.MapPath(Path));

                    FileUpload.SaveAs(Server.MapPath(UrlUtil.Combine(Path, fileName)));

                    Response.Redirect("List.aspx?path=" + Server.UrlEncode(Path));
                }
                else
                {
                    ((Management)Master).ShowMessage(String.Format(T("FileMgnt.NotSupportFileType{0}"), ext), MessageType.Error);
                }
            }
        }
    }
}