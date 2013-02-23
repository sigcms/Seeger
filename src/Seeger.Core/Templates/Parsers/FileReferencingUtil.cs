using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Templates.Parsers
{
    static class FileReferencingUtil
    {
        public static string GetReferencedFilePhysicalPath(string currentFilePhysicalPath, string referencedFileVirtualPath, string applicationBasePath)
        {
            if (IsFileName(referencedFileVirtualPath))
            {
                return Path.Combine(Path.GetDirectoryName(currentFilePhysicalPath), referencedFileVirtualPath);
            }

            if (referencedFileVirtualPath.StartsWith("~/"))
            {
                referencedFileVirtualPath = referencedFileVirtualPath.Substring(1);
            }

            if (referencedFileVirtualPath.StartsWith("/"))
            {
                return Path.Combine(applicationBasePath, referencedFileVirtualPath.Substring(1).Replace('/', '\\'));
            }

            if (!referencedFileVirtualPath.StartsWith("../"))
            {
                return Path.Combine(Path.GetDirectoryName(currentFilePhysicalPath), referencedFileVirtualPath.Replace('/', '\\'));
            }

            var segments = referencedFileVirtualPath.Split('/');
            var result = Path.GetDirectoryName(currentFilePhysicalPath);

            for (var i = 0; i < segments.Length; i++)
            {
                if (segments[i] == "..")
                {
                    result = Path.GetDirectoryName(result);
                }
                else
                {
                    result = Path.Combine(result, segments[i]);
                }
            }

            return result;
        }

        static bool IsFileName(string virtualPath)
        {
            return virtualPath.IndexOf('/') < 0;
        }
    }
}
