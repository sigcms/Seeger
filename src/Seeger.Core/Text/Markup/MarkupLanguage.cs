using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seeger.Text.Markup
{
    public static class MarkupLanguage
    {
        static readonly Dictionary<string, ITagProcessor> _processors = new Dictionary<string, ITagProcessor>(StringComparer.OrdinalIgnoreCase);

        static MarkupLanguage()
        {
            _processors.Add(Tags.T, new LocalizationTagProcessor());
        }

        static readonly Regex _tagPattern = new Regex(@"\<(?<tag>\w+)\>(?<content>[^\<]*)\</\k<tag>\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string Transform(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            var result = _tagPattern.Replace(text, match =>
                         {
                             var tag = match.Groups["tag"].Value;
                             if (_processors.ContainsKey(tag))
                             {
                                 return _processors[tag].Process(tag, match.Groups["content"].Value);
                             }

                             return match.Value;
                         });

            return result;
        }

        public static void RegisterTagProcessor(string tag, ITagProcessor processor)
        {
            lock (_processors)
            {
                if (_processors.ContainsKey(tag))
                {
                    _processors[tag] = processor;
                }
                else
                {
                    _processors.Add(tag, processor);
                }
            }
        }
    }
}
