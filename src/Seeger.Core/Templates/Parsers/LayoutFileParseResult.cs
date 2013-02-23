using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Templates.Parsers
{
    public class LayoutFileParseResult
    {
        public IList<string> MasterFilePaths { get; private set; }

        public IList<string> ZoneNames { get; private set; }

        public LayoutFileParseResult()
        {
            MasterFilePaths = new List<string>();
            ZoneNames = new List<string>();
        }
    }
}
