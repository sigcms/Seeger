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

        public IDirectory CreateFolder(string folderName)
        {
            Require.NotNullOrEmpty(folderName, "folderName");
            
            var directory = new DirectoryInfo(Path.Combine(Directory.FullName, folderName));
            if (directory.Exists)
                throw new InvalidOperationException("Folder \"" + folderName + "\" already exists.");

            directory.Create();

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
