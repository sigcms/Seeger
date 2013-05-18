using Newtonsoft.Json;
using Seeger.Files;
using Seeger.Globalization;
using Seeger.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Seeger.Web.UI.Admin.Handlers
{
    public class FileUpload : AuthRequiredHttpHandler
    {
        public override HandlerAuthMode AuthMode
        {
            get
            {
                return HandlerAuthMode.AuthByPostedToken;
            }
        }

        protected override void DoProcessRequest(HttpContext context)
        {
            try
            {
                DoUpload(context);
            }
            catch (Exception ex)
            {
                WriteResult(context, OperationResult.CreateErrorResult(ex));
            }
        }

        private void DoUpload(HttpContext context)
        {
            var folder = context.Request["folder"];

            if (String.IsNullOrEmpty(folder))
            {
                folder = "/Files";
            }

            if (!FileExplorer.AllowUploadPath(folder))
                throw new InvalidOperationException(ResourceFolder.Global.GetValue("Message.AccessDenied"));

            var file = context.Request.Files[0];
            var extension = Path.GetExtension(file.FileName);

            if (!FileExplorer.SupportFileExtension(extension))
                throw new InvalidOperationException("File extension is not supported: " + extension);

            var originalFileName = Path.GetFileName(file.FileName);
            var fileName = FileExplorer.CalculateFinalName(folder, FileExplorer.ApplySecurityFilterToFileName(originalFileName));
            var virtualPath = UrlUtil.Combine(folder, fileName);

            file.SaveAs(context.Server.MapPath(virtualPath));

            var result = new OperationResult
            {
                Success = true,
                Data = new FileUploadResult
                {
                    FileName = fileName,
                    VirtualPath = virtualPath
                }
            };

            WriteResult(context, result);
        }

        private void WriteResult(HttpContext context, OperationResult result)
        {
            context.Response.Write(JsonConvertUtil.CamelCaseSerializeObject(result));
        }

        protected override void OnAccessDenied(HttpContext context)
        {
            WriteResult(context, OperationResult.CreateErrorResult(ResourceFolder.Global.GetValue("Message.AccessDenied")));
        }

        public class FileUploadResult
        {
            public string FileName { get; set; }

            public string VirtualPath { get; set; }
        }
    }
}