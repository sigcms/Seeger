using Seeger.Text.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Seeger.Core.Tests.Text.Markup
{
    public class MarkupLanguageFacts
    {
        public class TheTransformMethod
        {
            [Fact]
            public void CanTransformTags()
            {
                MarkupLanguage.RegisterTagProcessor("t", new MockTagProcessor
                {
                    ProcessFunc = (tag, content) =>
                    {
                        return "[L]" + content + "[/L]";
                    }
                });

                var text = "<t>Hello</t>: world";
                var result = MarkupLanguage.Transform(text);

                Assert.Equal("[L]Hello[/L]: world", result);
            }
        }
    }
}
