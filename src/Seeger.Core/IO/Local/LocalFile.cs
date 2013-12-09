using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.IO.Local
{
    public class LocalFile : LocalFileSystemEntry, IFile
    {
        protected FileInfo File
        {
            get
            {
                return (FileInfo)base.FileSystemInfo;
            }
        }

        public long Length
        {
            get
            {
                return File.Length;
            }
        }

        public DateTime CreationTimeUtc
        {
            get
            {
                return File.CreationTimeUtc;
            }
        }

        public DateTime LastWriteTimeUtc
        {
            get
            {
                return File.LastWriteTimeUtc;
            }
        }

        public LocalFile(FileInfo file, LocalFileSystem fileSystem)
            : base(file, fileSystem)
        {
        }

        public System.IO.Stream OpenRead()
        {
            return File.OpenRead();
        }

        public System.IO.Stream OpenWrite()
        {
            return File.OpenWrite();
        }
    }
}
