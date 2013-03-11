using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.Processors
{
    public class IgnoredPathProcessor : IHttpProcessor
    {
        static readonly HashSet<string> _ignoredExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".aspx", ".ashx", ".swf", ".html", ".htm", ".axd", ".jpg", ".png", ".gif", ".ico", ".js", ".css" };
        static readonly HashSet<string> _ignoredFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Admin", "Plugins", "Templates", "Scripts", "App_Themes" };

        public void Process(HttpProcessingContext context)
        {
            if (context.RemainingSegments.Count > 0 && _ignoredFolders.Contains(context.RemainingSegments[0]))
            {
                context.StopProcessing = true;
                return;
            }

            if (!String.IsNullOrEmpty(context.FileExtension) && _ignoredExtensions.Contains(context.FileExtension))
            {
                context.StopProcessing = true;
                return;
            }
        }
    }
}
