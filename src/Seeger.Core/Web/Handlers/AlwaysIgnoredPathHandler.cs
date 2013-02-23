using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seeger.Web.Handlers
{
    class AlwaysIgnoredPathHandler : IRequestHandler
    {
        public static readonly AlwaysIgnoredPathHandler Instance = new AlwaysIgnoredPathHandler();

        private static readonly List<Regex> _ignoredPatterns = new List<Regex>();

        static AlwaysIgnoredPathHandler()
        {
            _ignoredPatterns.Add(new Regex(@"^/Admin", RegexOptions.Compiled | RegexOptions.IgnoreCase));
            _ignoredPatterns.Add(new Regex(@"(WebResource|ScriptResource).axd", RegexOptions.Compiled | RegexOptions.IgnoreCase));
        }

        public void Handle(RequestHandlerContext context)
        {
            bool ignore = false;

            foreach (var pattern in _ignoredPatterns)
            {
                if (pattern.IsMatch(context.TargetPath))
                {
                    ignore = true;
                    break;
                }
            }

            if (!ignore)
            {
                CultureHandler.Instance.Handle(context);
            }
        }
    }
}
