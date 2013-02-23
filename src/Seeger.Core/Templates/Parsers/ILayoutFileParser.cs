using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Templates.Parsers
{
    public interface ILayoutFileParser
    {
        LayoutFileParseResult Parse(string layoutFilePath);
    }

    public static class LayoutFileParsers
    {
        public static ILayoutFileParser GetParser(string layoutFileExtension)
        {
            if (layoutFileExtension == ".aspx")
            {
                return new WebFormLayoutParser();
            }

            throw new NotSupportedException("Not support layout file type: " + layoutFileExtension);
        }
    }
}
