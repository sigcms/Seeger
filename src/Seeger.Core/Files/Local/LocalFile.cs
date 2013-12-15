using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files.Local
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

        public Stream Read()
        {
            return File.OpenRead();
        }

        public void Write(Stream stream)
        {
            using (var fs = File.OpenWrite())
            {
                stream.WriteTo(fs);
            }
        }
    }
}
