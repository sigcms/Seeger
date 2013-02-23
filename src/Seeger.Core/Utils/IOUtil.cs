using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seeger
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

        public static void EnsureFileDeleted(string filename)
        {
            Require.NotNullOrEmpty(filename, "path");

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}
