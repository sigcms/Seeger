using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Files.Indexing
{
    public class FileIndexEntry : FileSystemIndexEntry
    {
        public DateTime CreationTimeUtc { get; set; }

        public DateTime LastWriteTimeUtc { get; set; }
    }
}
