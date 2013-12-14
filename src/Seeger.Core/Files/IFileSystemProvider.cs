using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public interface IFileSystemProvider
    {
        string Name { get; }

        string GetConfigurationUrl(string bucketId);

        IFileSystem CreateFileSystem(NameValueCollection config);
    }
}
