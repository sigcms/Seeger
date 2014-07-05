using Newtonsoft.Json;
using Seeger.Files;
using Seeger.Globalization;
using Seeger.Logging;
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
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

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
                _log.ErrorException(UserReference.System(), ex, "Fail upload file. " + ex.Message);
                WriteResult(context, OperationResult.CreateErrorResult(ex));
            }
        }

        private void DoUpload(HttpContext context)
        {
            var folder = context.Request["folder"];

            if (String.IsNullOrEmpty(folder))
            {
                folder = "/";
            }

            var file = context.Request.Files[0];
            var autoRename = context.Request["autoRename"] == "true";

            FileBucketMeta meta = null;

            var bucketId = context.Request["bucketId"];

            if (!String.IsNullOrWhiteSpace(bucketId))
            {
                meta = FileBucketMetaStores.Current.Load(bucketId);
            }
            else
            {
                meta = FileBucketMetaStores.Current.LoadDefault();
            }

            var fileSystem = FileSystemProviders.Get(meta.FileSystemProviderName).LoadFileSystem(meta);
            var directory = fileSystem.GetDirectory(folder);

            if (directory == null)
            {
                directory = fileSystem.CreateDirectory(folder);
            }

            string fileName = null;

            if (autoRename)
            {
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
            }
            else
            {
                fileName = directory.ComputeUniqueFileName(file.FileName);
            }

            var virtualFile = directory.CreateFile(fileName);
            virtualFile.Write(file.InputStream);
            
            var result = new OperationResult
            {
                Success = true,
                Data = new FileUploadResult
                {
                    FileName = fileName,
                    VirtualPath = virtualFile.PublicUri,
                    PublicUri = virtualFile.PublicUri
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

            [Obsolete("Use PublicUri instead.")]
            public string VirtualPath { get; set; }

            public string PublicUri { get; set; }
        }
    }
}