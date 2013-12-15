using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public static class DirectoryExtensions
    {
        public static string ComputeUniqueFileName(this IDirectory container, string candidateFileName)
        {
            candidateFileName = Path.GetFileName(candidateFileName);

            if (!container.Exists)
            {
                return candidateFileName;
            }

            var ext = Path.GetExtension(candidateFileName);
            var candidateNameWithoutExt = Path.GetFileNameWithoutExtension(candidateFileName);
            var finalNameWithoutExt = candidateNameWithoutExt;

            var i = 1;

            while (true)
            {
                var file = container.GetFile(finalNameWithoutExt + ext);

                if (file == null)
                {
                    break;
                }

                finalNameWithoutExt = candidateNameWithoutExt + "(" + i + ")";
                ++i;
            }

            return finalNameWithoutExt + ext;
        }
    }
}
