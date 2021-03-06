﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files.Indexing
{
    public abstract class FileSystemIndexEntry
    {
        public string Name { get; set; }

        public string Extension
        {
            get
            {
                return Path.GetExtension(Name);
            }
        }

        public long Length { get; set; }
    }
}
