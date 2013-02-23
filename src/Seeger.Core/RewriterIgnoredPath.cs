using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Seeger
{
    public class RewriterIgnoredPath
    {
        [EntityKey]
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Path { get; set; }
        public virtual bool MatchByRegex { get; set; }
        public virtual bool Reserved { get; set; }

        public RewriterIgnoredPath()
        {
            Name = String.Empty;
            MatchByRegex = true;
        }

        public virtual bool Test(HttpRequest request)
        {
            Require.NotNull(request, "request");

            if (MatchByRegex)
            {
                return Regex.IsMatch(request.Url.AbsoluteUri, Path);
            }

            if (IsAbsoluteUrl(Path))
            {
                return Path.IgnoreCaseEquals(request.Url.AbsoluteUri);
            }

            return Path.IgnoreCaseEquals(request.Path);
        }

        private bool IsAbsoluteUrl(string url)
        {
            return url.IndexOf("://") > 0;
        }
    }
}
