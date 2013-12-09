using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.IO
{
    public interface IFile : IFileSystemEntry
    {
        long Length { get; }

        DateTime CreationTimeUtc { get; }

        DateTime LastWriteTimeUtc { get; }

        Stream OpenRead();

        Stream OpenWrite();
    }
}
