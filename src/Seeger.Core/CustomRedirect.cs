using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seeger
{
    public class CustomRedirect
    {
        [EntityKey]
        public virtual int Id { get; protected set; }

        public virtual string From { get; set; }

        public virtual string To { get; set; }

        public virtual string Description { get; set; }

        public virtual bool MatchByRegex { get; set; }

        public virtual RedirectMode RedirectMode { get; set; }

        public CustomRedirect()
        {
            Description = String.Empty;
        }

        public virtual bool IsMatch(System.Web.HttpRequest request)
        {
            Require.NotNull(request, "request");

            if (MatchByRegex)
            {
                return Regex.IsMatch(request.Url.AbsoluteUri, From);
            }

            if (IsAbsoluteUrl(From))
            {
                return From.IgnoreCaseEquals(request.Url.AbsoluteUri);
            }

            return From.IgnoreCaseEquals(request.Path);
        }

        private bool IsAbsoluteUrl(string url)
        {
            return url.IndexOf("://") > 0;
        }

    }

}
