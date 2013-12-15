using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger
{
    static class StreamExtensions
    {
        public static void WriteTo(this Stream source, Stream target)
        {
            var count = 0;
            var buffer = new byte[2048];

            while (true)
            {
                count = source.Read(buffer, 0, buffer.Length);
                if (count > 0)
                {
                    target.Write(buffer, 0, count);
                }
                else
                {
                    break;
                }
            }

            target.Flush();
        }
    }
}
