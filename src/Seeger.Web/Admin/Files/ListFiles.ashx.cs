using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Seeger.Files;

namespace Seeger.Web.UI.Admin.Files
{
    public class ListFiles : IHttpHandler
    {
        private HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        private string _directory;
        private bool _isRoot;
        private bool _imageOnly;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            _directory = context.Request.QueryString["dir"].AsString().TrimEnd('/');
            _imageOnly = context.Request.QueryString["imageOnly"] == "1";
            _isRoot = FileExplorer.IsAllowedUploadRootPath(_directory);

            if (!FileExplorer.AllowUploadPath(_directory))
            {
                return;
            }

            string[] extensions = context.Request.QueryString["extensions"].AsString().SplitWithoutEmptyEntries(',');

            if (_imageOnly)
            {
                WriteHtml(FileExplorer.ListByPredicate(_directory, entry => entry.IsDirectory || FileType.IsImageFile(entry.Name)));
            }
            else if (extensions.Length > 0)
            {
                WriteHtml(FileExplorer.List(_directory, true, extensions));
            }
            else
            {
                WriteHtml(FileExplorer.List(_directory));
            }
        }

        private void WriteHtml(IEnumerable<FileSystemEntry> items)
        {
            Response.Write("<div class='fb-folder-panel'>");
            WriteHtmlForDirectories(items.Where(it => it.IsDirectory));
            Response.Write("</div>");

            Response.Write("<div class='fb-file-panel'>");
            WriteHtmlForFiles(items.Where(it => !it.IsDirectory));
            Response.Write("<div>");

            Response.Write("<div style='clear:both'></div>");
        }

        private void WriteHtmlForFiles(IEnumerable<FileSystemEntry> files)
        {
            Response.Write("<ul>");

            foreach (var file in files)
            {
                Response.Write(String.Format("<li><a href='#' rel='{0}' class='fb-file fb-{1}'>{2}</a></li>",
                    file.VirtualPath,
                    file.Extension.Length > 0 ? file.Extension.Substring(1).ToLower() : String.Empty,
                    file.Name));
            }

            Response.Write("</ul>");
        }

        private void WriteHtmlForDirectories(IEnumerable<FileSystemEntry> directories)
        {
            Response.Write("<ul>");

            if (!_isRoot)
            {
                Response.Write(String.Format("<li><a href='#' rel='{0}' class='fb-folder'>..</a></li>", UrlUtility.GetParentPath(_directory)));
            }

            foreach (var dir in directories)
            {
                Response.Write(String.Format("<li><a href='#' rel='{0}' class='fb-folder'>{1}</a></li>", dir.VirtualPath, dir.Name));
            }

            Response.Write("</ul>");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}