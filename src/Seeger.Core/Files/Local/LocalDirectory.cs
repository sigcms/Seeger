using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files.Local
{
    public class LocalDirectory : LocalFileSystemEntry, IDirectory
    {
        protected DirectoryInfo Directory
        {
            get
            {
                return (DirectoryInfo)base.FileSystemInfo;
            }
        }

        public LocalDirectory(DirectoryInfo directory, LocalFileSystem fileSystem)
            : base(directory, fileSystem)
        {
        }

        public void Create()
        {
            Directory.Create();
        }

        public IDirectory CreateSubdirectory(string folderName)
        {
            Require.NotNullOrEmpty(folderName, "folderName");

            if (!Directory.Exists)
            {
                Directory.Create();
            }

            var directory = new DirectoryInfo(Path.Combine(Directory.FullName, folderName));

            if (!directory.Exists)
            {
                directory.Create();
            }

            return new LocalDirectory(directory, FileSystem);
        }

        public IDirectory GetDirectory(string folderName)
        {
            if (!Directory.Exists)
            {
                return null;
            }

            var directory = Directory.GetDirectories(folderName, SearchOption.TopDirectoryOnly)
                                     .FirstOrDefault(x => x.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase));

            if (directory == null)
            {
                return null;
            }

            return new LocalDirectory(directory, FileSystem);
        }

        public IEnumerable<IDirectory> GetDirectories()
        {
            if (Directory.Exists)
            {
                foreach (var directory in Directory.GetDirectories())
                {
                    yield return new LocalDirectory(directory, FileSystem);
                }
            }
        }

        public IFile CreateFile(string fileName)
        {
            if (!Directory.Exists)
            {
                Directory.Create();
            }

            var file = new FileInfo(Path.Combine(Directory.FullName, fileName));

            return new LocalFile(file, FileSystem);
        }

        public IFile GetFile(string fileName)
        {
            if (!Directory.Exists)
            {
                return null;
            }

            var file = Directory.GetFiles(fileName, SearchOption.TopDirectoryOnly)
                                .FirstOrDefault(x => x.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase));

            if (file == null)
            {
                return null;
            }

            return new LocalFile(file, FileSystem);
        }

        public IEnumerable<IFile> GetFiles()
        {
            if (Directory.Exists)
            {
                foreach (var file in Directory.GetFiles())
                {
                    yield return new LocalFile(file, FileSystem);
                }
            }
        }
    }
}
