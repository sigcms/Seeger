using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public interface IFile : IFileSystemEntry
    {
        long Length { get; }

        DateTime CreationTimeUtc { get; }

        DateTime LastWriteTimeUtc { get; }

        Stream Read();

        void Write(Stream stream);
    }
}
