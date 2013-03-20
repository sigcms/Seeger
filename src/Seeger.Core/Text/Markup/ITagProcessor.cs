using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Text.Markup
{
    public interface ITagProcessor
    {
        string Process(string tagName, string content);
    }
}
