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
            var folder = context.Request["folder"];

            if (String.IsNullOrEmpty(folder))
            {
                folder = "/Files";
            }

            if (!FileExplorer.AllowUploadPath(folder))
            {
                WriteResult(context, OperationResult.CreateErrorResult(ResourceFolder.Global.GetValue("Message.AccessDefined")));
                return;
            }

            var file = context.Request.Files[0];
            var extension = Path.GetExtension(file.FileName);

            if (!FileExplorer.SupportFileExtension(extension))
            {
                WriteResult(context, OperationResult.CreateErrorResult("File extension is not supported: " + extension));
                return;
            }

            var originalFileName = Path.GetFileName(file.FileName);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
            var virtualPath = UrlUtil.Combine(folder, fileName);

            try
            {
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
            catch (Exception ex)
            {
                WriteResult(context, new OperationResult
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new FileUploadResult
                    {
                        FileName = originalFileName,
                        VirtualPath = UrlUtil.Combine(folder, originalFileName)
                    }
                });
            }
        }

        private void WriteResult(HttpContext context, OperationResult result)
        {
            context.Response.Write(JsonConvertUtil.CamelCaseSerializeObject(result));
        }

        public class FileUploadResult
        {
            public string FileName { get; set; }

            public string VirtualPath { get; set; }
        }
    }
}