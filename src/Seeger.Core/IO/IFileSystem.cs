using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.IO
{
    public interface IFileSystem
    {
        IDirectory RootDirectory { get; }

        IDirectory GetDirectory(string virtualPath);
    }
}
