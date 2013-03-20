using Seeger.Text.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Core.Tests.Text.Markup
{
    public class MockTagProcessor : ITagProcessor
    {
        public Func<string, string, string> ProcessFunc = null;

        public string Process(string tagName, string content)
        {
            if (ProcessFunc != null)
            {
                return ProcessFunc(tagName, content);
            }

            return content;
        }
    }
}
