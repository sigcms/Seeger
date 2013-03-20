using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Text.Markup
{
    public class LocalizationTagProcessor : ITagProcessor
    {
        public string Process(string tagName, string content)
        {
            return ResourcesFolder.Global.GetValue(content) ?? content;
        }
    }
}
