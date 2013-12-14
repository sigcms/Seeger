using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public interface IDirectory : IFileSystemEntry
    {
        void Create();

        IDirectory CreateFolder(string folderName);

        IEnumerable<IDirectory> GetDirectories();

        IEnumerable<IFile> GetFiles();
    }
}
