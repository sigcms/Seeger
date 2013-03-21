using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Seeger.Web;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Seeger.Files
{
    public class FileExplorer
    {
        private static readonly HashSet<string> _deniedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static readonly List<string> _allowedUploadPaths = new List<string>();

        internal static void ConfigureWith(XElement xml)
        {
            var deniedExtensions = xml.ChildElementValue("denied-extensions", String.Empty)
                                      .SplitWithoutEmptyEntries(',')
                                      .Select(x => x.Trim())
                                      .ToList();

            if (deniedExtensions.Count > 0)
            {
                _deniedExtensions.Clear();

                foreach (var ext in deniedExtensions)
                {
                    string extension = ext.Trim();
                    if (extension.Length > 0)
                    {
                        if (!extension.StartsWith("."))
                        {
                            extension = "." + extension;
                        }

                        _deniedExtensions.Add(extension);
                    }
                }
            }

            var allowedUploadPaths = xml.ChildElementValue("allowed-upload-paths", String.Empty)
                                        .SplitWithoutEmptyEntries(',')
                                        .Select(x => x.Trim())
                                        .ToList();

            if (allowedUploadPaths.Count > 0)
            {
                _allowedUploadPaths.Clear();

                foreach (var path in allowedUploadPaths)
                {
                    string allowedPath = path.Trim();
                    if (allowedPath.Length > 0)
                    {
                        if (!allowedPath.EndsWith("/"))
                        {
                            allowedPath += "/";
                        }

                        _allowedUploadPaths.Add(allowedPath);
                    }
                }
            }
        }

        public static IEnumerable<string> DeniedExtensions
        {
            get { return _deniedExtensions; }
        }

        public static IEnumerable<string> AllowedUploadPaths
        {
            get { return _allowedUploadPaths; }
        }

        public static bool IsAllowedUploadRootPath(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            if (!virtualPath.EndsWith("/"))
            {
                virtualPath += "/";
            }

            return _allowedUploadPaths.Any(it => it.IgnoreCaseEquals(virtualPath));
        }

        public static bool SupportFileExtension(string extension)
        {
            Require.NotNullOrEmpty(extension, "extension");

            extension = extension.Trim();

            return !_deniedExtensions.Contains(extension.StartsWith(".") ? extension : "." + extension);
        }

        public static bool AllowUploadPath(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            virtualPath = virtualPath.Trim();
            if (!virtualPath.EndsWith("/"))
            {
                virtualPath += "/";
            }

            foreach (var path in _allowedUploadPaths)
            {
                if (virtualPath.IgnoreCaseStartsWith(path))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ApplySecurityFilterToFileName(string fileName)
        {
            return SecurityFileNameFilter.Instance.Filter(fileName);
        }

        public static string CalculateFinalName(string containerVirtualPath, string newName)
        {
            Require.NotNullOrEmpty(containerVirtualPath, "containerVirtualPath");

            return CalculateFinalName(new DirectoryInfo(Server.MapPath(containerVirtualPath)), newName);
        }

        public static string CalculateFinalName(DirectoryInfo container, string newName)
        {
            Require.NotNull(container, "container");
            Require.NotNullOrEmpty(newName, "newName");

            if (!container.Exists)
            {
                return newName;
            }

            string ext = Path.GetExtension(newName);
            string newNameWithoutExt = newName.Substring(0, newName.Length - ext.Length);
            string finalNameWithoutExt = newNameWithoutExt;

            var infos = container.GetFileSystemInfos(ext.Length > 0 ? finalNameWithoutExt + ext : finalNameWithoutExt);
            int i = 1;

            while (infos.Length == 1)
            {
                finalNameWithoutExt = newNameWithoutExt + "(" + i + ")";

                infos = container.GetFileSystemInfos(ext.Length > 0 ? finalNameWithoutExt + ext : finalNameWithoutExt);
                ++i;
            }

            return finalNameWithoutExt + ext;
        }

        public static ICollection<FileSystemEntry> List(string virtualPath)
        {
            return ListByPredicate(virtualPath, null);
        }

        public static ICollection<FileSystemEntry> List(string virtualPath, bool listDirectory, params string[] extensions)
        {
            if (extensions != null && extensions.Length > 0)
            {
                HashSet<string> extSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var ext in extensions)
                {
                    string extension = ext.Trim();

                    if (extension.Length > 0)
                    {
                        if (!extension.StartsWith("."))
                        {
                            extension = "." + extension;
                        }

                        extSet.Add(extension);
                    }
                }

                if (extSet.Count > 0)
                {
                    if (listDirectory)
                    {
                        return ListByPredicate(virtualPath, f => f.IsDirectory || extSet.Contains(f.Extension));
                    }

                    return ListByPredicate(virtualPath, f => !f.IsDirectory || extSet.Contains(f.Extension));
                }
            }

            if (listDirectory)
            {
                return List(virtualPath);
            }

            return ListByPredicate(virtualPath, f => !f.IsDirectory);
        }

        public static ICollection<FileSystemEntry> ListByPredicate(string virtualPath, Func<FileSystemEntry, bool> predicate)
        {
            List<FileSystemEntry> items = new List<FileSystemEntry>();

            var directory = new DirectoryInfo(Server.MapPath(virtualPath));
            if (directory.Exists)
            {
                foreach (var dir in directory.GetDirectories())
                {
                    var entry = FileSystemEntry.FromDirectory(dir, UrlUtil.Combine(virtualPath, dir.Name));

                    if (predicate == null || predicate(entry))
                    {
                        items.Add(entry);
                    }
                }

                foreach (var file in directory.GetFiles())
                {
                    if (!_deniedExtensions.Contains(file.Extension))
                    {
                        var entry = FileSystemEntry.FromFile(file, UrlUtil.Combine(virtualPath, file.Name));

                        if (predicate == null || predicate(entry))
                        {
                            items.Add(entry);
                        }
                    }
                }
            }

            return items;
        }
    }
}
