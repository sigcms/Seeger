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
        public string VirtualPath { get; internal set; }
        public string FullName { get; private set; }
        public string Extension { get; private set; }
        public bool IsDirectory { get; private set; }
        public bool IsHidden { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime LastWriteTime { get; private set; }

        private FileSystemEntry()
        {
            Extension = String.Empty;
        }

        public static FileSystemEntry FromFile(FileInfo file)
        {
            Require.NotNull(file, "file");

            return new FileSystemEntry
            {
                Name = file.Name,
                FullName = file.FullName,
                IsDirectory = false,
                IsHidden = file.IsHidden(),
                Extension = file.Extension,
                CreationTime = file.CreationTime,
                LastWriteTime = file.LastWriteTime
            };
        }

        public static FileSystemEntry FromDirectory(DirectoryInfo directory)
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
