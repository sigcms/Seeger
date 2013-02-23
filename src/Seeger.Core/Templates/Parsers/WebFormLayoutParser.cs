using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.RegularExpressions;

namespace Seeger.Templates.Parsers
{
    public class WebFormLayoutParser : ILayoutFileParser
    {
        static readonly DirectiveRegex _directiveRegex = new DirectiveRegex();
        static readonly TagRegex _startTagRegex = new TagRegex();
        static readonly EndTagRegex _endTagRegex = new EndTagRegex();
        static readonly CommentRegex _commentRegex = new CommentRegex();
        static readonly AspCodeRegex _codeRegex = new AspCodeRegex();

        public Func<string, string> ReadFileContent { get; set; }

        public string ApplicationBasePath { get; private set; }

        public WebFormLayoutParser()
            : this(AppDomain.CurrentDomain.BaseDirectory)
        {
        }

        public WebFormLayoutParser(string applicationBasePath)
        {
            ReadFileContent = path => File.ReadAllText(path);
            ApplicationBasePath = applicationBasePath;
        }

        public LayoutFileParseResult Parse(string layoutFilePath)
        {
            var result = new LayoutFileParseResult();
            DoParse(layoutFilePath, result);
            return result;
        }

        private void DoParse(string layoutFilePath, LayoutFileParseResult result)
        {
            var index = 0;
            string masterFilePath = null;

            var fileContent = ReadFileContent(layoutFilePath).Trim();
            fileContent = _commentRegex.Replace(fileContent, String.Empty);
            fileContent = _codeRegex.Replace(fileContent, String.Empty);

            while (index < fileContent.Length)
            {
                if (String.IsNullOrEmpty(masterFilePath))
                {
                    index = ParseMasterReference(fileContent, index, out masterFilePath);

                    if (!String.IsNullOrEmpty(masterFilePath))
                    {
                        masterFilePath = FileReferencingUtil.GetReferencedFilePhysicalPath(layoutFilePath, masterFilePath, ApplicationBasePath);
                        result.MasterFilePaths.Add(masterFilePath);
                    }
                }

                // find zones
                var m = _startTagRegex.Match(fileContent, index);

                if (m.Success)
                {
                    index = m.Index + m.Length;

                    var tagName = m.Groups["tagname"].Value;

                    if (!String.IsNullOrEmpty(tagName) && tagName.IgnoreCaseEquals("sig:ZoneControl"))
                    {
                        var attrNameGroup = m.Groups["attrname"];
                        var hasRunAtServer = false;
                        string zoneName = null;

                        for (var i = 0; i < attrNameGroup.Captures.Count; i++)
                        {
                            if (attrNameGroup.Captures[i].Value.IgnoreCaseEquals("runat"))
                            {
                                hasRunAtServer = m.Groups["attrval"].Captures[i].Value.IgnoreCaseEquals("server");
                            }
                            else if (attrNameGroup.Captures[i].Value.IgnoreCaseEquals("ZoneName"))
                            {
                                zoneName = m.Groups["attrval"].Captures[i].Value;
                                if (hasRunAtServer) break;
                            }
                        }

                        if (hasRunAtServer)
                        {
                            result.ZoneNames.Add(zoneName);
                        }
                    }

                    continue;
                }

                m = _endTagRegex.Match(fileContent, index);

                if (m.Success)
                {
                    index = m.Index + m.Length;
                    continue;
                }

                ++index;
            }

            if (!String.IsNullOrEmpty(masterFilePath))
            {
                DoParse(masterFilePath, result);
            }
        }

        private int ParseMasterReference(string layoutFileContent, int start, out string master)
        {
            master = null;

            var match = _directiveRegex.Match(layoutFileContent, start);

            while (match.Success)
            {
                var attrNameGroup = match.Groups["attrname"];

                if (attrNameGroup.Captures[0].Value.IgnoreCaseEquals("Page")
                    || attrNameGroup.Captures[0].Value.IgnoreCaseEquals("Master"))
                {
                    for (var i = 1; i < attrNameGroup.Captures.Count; i++)
                    {
                        if (attrNameGroup.Captures[i].Value.IgnoreCaseEquals("MasterPageFile"))
                        {
                            master = match.Groups["attrval"].Captures[i].Value;
                        }
                    }
                }

                start = start + match.Length;
                match = _directiveRegex.Match(layoutFileContent, start);
            }

            return start;
        }
    }
}
