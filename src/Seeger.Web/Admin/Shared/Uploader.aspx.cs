using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;

using Seeger.Files;

namespace Seeger.Web.UI.Admin.Shared
{
    public partial class Uploader : AdminPageBase
    {
        protected bool ImageOnly
        {
            get
            {
                return Request.QueryString["imageOnly"] == "1";
            }
        }

        protected string ImageFileNamePattern
        {
            get { return FileType.ImageFileNamePattern.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitUploader();
            }
            else
            {
                UploadFile();
            }
        }

        private void InitUploader()
        {
            int width = 0;
            int.TryParse(Request.QueryString["width"], out width);
            if (width > 0)
            {
                FileUpload.Width = width;
            }
        }

        private void UploadFile()
        {
            string dir = Request.QueryString["dir"];

            if (String.IsNullOrEmpty(dir) || !FileExplorer.AllowUploadPath(dir))
                throw new InvalidOperationException("'dir' is empty or is not valid upload path.");

            if (ImageOnly)
            {
                if (!FileType.IsImageFile(FileUpload.FileName))
                    throw new InvalidOperationException("File type not supported.");
            }

            string fileName = FileExplorer.ApplySecurityFilterToFileName(FileUpload.FileName);

            fileName = FileExplorer.CalculateFinalName(dir, Path.GetFileName(fileName));

            IOUtil.EnsureDirectoryCreated(Server.MapPath(dir));

            FileUpload.SaveAs(Server.MapPath(UrlUtility.Combine(dir, fileName)));

            ClientScript.RegisterClientScriptBlock(GetType(), "Uploaded", String.Format("onUploaded('{0}', '{1}');", dir, fileName), true);
        }

        protected override IEnumerable<string> GetCssFilePaths()
        {
            return Enumerable.Empty<string>();
        }
    }
}