using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public interface IDirectory : IFileSystemEntry
    {
        void Create();

        IDirectory CreateSubdirectory(string folderName);

        IDirectory GetDirectory(string folderName);

        IEnumerable<IDirectory> GetDirectories();

        IFile CreateFile(string fileName);

        IFile GetFile(string fileName);

        IEnumerable<IFile> GetFiles();
    }
}
