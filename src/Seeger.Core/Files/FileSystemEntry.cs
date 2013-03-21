using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seeger.Files
{
    public class FileSystemEntry
    {
        public string Name { get; private set; }
        public string VirtualPath { get; private set; }
        public string FullName { get; private set; }
        public string Extension { get; private set; }
        public long Length { get; private set; }
        public bool IsDirectory { get; private set; }
        public bool IsHidden { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime LastWriteTime { get; private set; }

        private FileSystemEntry()
        {
        }

        public static FileSystemEntry FromFile(FileInfo file, string virtualPath)
        {
            Require.NotNull(file, "file");
            return new FileSystemEntry
            {
                Name = file.Name,
                FullName = file.FullName,
                IsDirectory = false,
                IsHidden = file.IsHidden(),
                Length = file.Length,
                Extension = file.Extension,
                CreationTime = file.CreationTime,
                LastWriteTime = file.LastWriteTime
            };
        }

        public static FileSystemEntry FromDirectory(DirectoryInfo directory, string virtualPath)
        {
            Require.NotNull(directory, "directory");

            return new FileSystemEntry
            {
                Name = directory.Name,
                FullName = directory.FullName,
                IsDirectory = true,
                IsHidden = directory.IsHidden(),
                CreationTime = directory.CreationTime,
                LastWriteTime = directory.LastWriteTime
            };
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
