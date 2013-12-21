using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public interface IFileSystem
    {
        IDirectory RootDirectory { get; }

        IDirectory GetDirectory(string virtualPath);

        IDirectory CreateDirectory(string virtualPath);
    }
}
