using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.IO
{
    public interface IDirectory : IFileSystemEntry
    {
        IEnumerable<IDirectory> GetDirectories();

        IEnumerable<IFile> GetFiles();
    }
}
