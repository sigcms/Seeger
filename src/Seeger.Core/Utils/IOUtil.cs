using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seeger.Utils
{
    public static class IOUtil
    {
        public static void EnsureDirectoryCreated(string path)
        {
            Require.NotNullOrEmpty(path, "path");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void EnsureDirectoryDeleted(string path)
        {
            Require.NotNullOrEmpty(path, "path");

            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }

        public static void EnsureFileDeleted(string path)
        {
            Require.NotNullOrEmpty(path, "path");

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
