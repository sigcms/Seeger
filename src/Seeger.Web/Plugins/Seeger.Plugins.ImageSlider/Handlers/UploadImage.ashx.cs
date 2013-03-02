using Newtonsoft.Json;
using Seeger.Files;
using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Plugins.ImageSlider.Utils;
using Seeger.Utils;
using Seeger.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.ImageSlider.Handlers
{
    public class UploadImage : AuthRequiredHttpHandler
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
                context.Response.Write(
                    JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        Message = ex.Message
                    }));
            }

        }

        private static void DoUpload(HttpContext context)
        {
            var file = context.Request.Files[0];

            if (!FileExplorer.SupportFileExtension(Path.GetExtension(file.FileName)))
                throw new InvalidOperationException("File type not supported.");

            var folderVirtualPath = "/Files/Temp";

            IOUtil.EnsureDirectoryCreated(context.Server.MapPath(folderVirtualPath));

            var fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Path.GetExtension(file.FileName);

            var imageVirutalPath = UrlUtility.Combine(folderVirtualPath, fileName);
            var thumbVirutalPath = UrlUtility.Combine(folderVirtualPath, "min-" + fileName);

            file.SaveAs(context.Server.MapPath(imageVirutalPath));

            ImageUtility.CreateThumbnail(context.Server.MapPath(imageVirutalPath), context.Server.MapPath(thumbVirutalPath), 85, ThumbLagerThanOriginalImageBehavior.ZoomOut);

            context.Response.Write(JsonConvert.SerializeObject(new
            {
                Success = true,
                SliderItemViewModel = JsonConvertUtil.CamelCaseSerializeObject(new SliderItem
                {
                    ImageUrl = imageVirutalPath
                })
            }));
        }
    }
}