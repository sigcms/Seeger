using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Files.Indexing
{
    public interface IFileSystemIndex
    {
        IEnumerable<FileIndexEntry> GetFiles(string virtualPath);

        void AddFile(string directoryVirtualPath, FileIndexEntry file);

        void UpdateFile(string directoryVirtualPath, FileIndexEntry file);

        void DeleteFile(string virtualPath);

        IEnumerable<DirectoryIndexEntry> GetDirectories(string virtualPath);

        void AddDirectory(string virtualPath);

        void DeleteDirectory(string virtualPath);
    }
}
