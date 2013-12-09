using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.IO
{
    public interface IFileSystemEntry
    {
        string VirtualPath { get; }

        string PublicUri { get; }

        string Name { get; }

        string Extension { get; }

        bool Exists { get; }
    }
}
