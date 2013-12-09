using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.IO.Local
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

        public IEnumerable<IDirectory> GetDirectories()
        {
            foreach (var directory in Directory.GetDirectories())
            {
                yield return new LocalDirectory(directory, FileSystem);
            }
        }

        public IEnumerable<IFile> GetFiles()
        {
            foreach (var file in Directory.GetFiles())
            {
                yield return new LocalFile(file, FileSystem);
            }
        }
    }
}
