using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Seeger.Files
{
    class SecurityFileNameFilter : IFileNameFilter
    {
        public static readonly SecurityFileNameFilter Instance = new SecurityFileNameFilter();

        private static readonly Regex _regex = new Regex(@"\.(asp|cer)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Filter(string fileName)
        {
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            nameWithoutExt = _regex.Replace(nameWithoutExt, "_");

            return nameWithoutExt + Path.GetExtension(fileName);
        }
    }
}
