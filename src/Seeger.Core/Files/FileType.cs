using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seeger.Files
{
    public class FileType
    {
        public static readonly Regex ImageFileNamePattern = new Regex(@"^.*\.(jpg|gif|png|bmp)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsImageFile(string fileName)
        {
            Require.NotNullOrEmpty(fileName, "fileName");

            return ImageFileNamePattern.IsMatch(fileName);
        }
    }
}
