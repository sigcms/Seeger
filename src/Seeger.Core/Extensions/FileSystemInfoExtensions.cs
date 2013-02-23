using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seeger
{
    public static class FileSystemInfoExtensions
    {
        public static bool IsHidden(this FileSystemInfo fileSystemInfo)
        {
            Require.NotNull(fileSystemInfo, "fileSystemInfo");

            return (fileSystemInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
        }
    }
}
