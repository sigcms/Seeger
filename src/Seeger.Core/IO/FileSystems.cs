using Seeger.IO.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.IO
{
    public static class FileSystems
    {
        public static IFileSystem Current { get; set; }

        static FileSystems()
        {
            Current = new LocalFileSystem("/Files/");
        }
    }
}
