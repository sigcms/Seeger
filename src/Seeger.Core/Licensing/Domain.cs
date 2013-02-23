using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seeger.Licensing
{
    class Domain
    {
        private static readonly Regex _ipv4Pattern = new Regex(@"^[0-9]{1,3}(\.[0-9]{1,3}){3}$", RegexOptions.Compiled);
        
        public static bool IsLocalHost(string domain)
        {
            if (String.IsNullOrEmpty(domain))
                throw new ArgumentException("'domain' is required.");

            return domain.Equals("127.0.0.1") || domain.Equals("localhost", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsIPv4Address(string domain)
        {
            if (String.IsNullOrEmpty(domain))
                throw new ArgumentException("'domain' is required.");

            return _ipv4Pattern.IsMatch(domain);
        }

        public static string Remove3W(string domain)
        {
            if (String.IsNullOrEmpty(domain))
                throw new ArgumentException("'domain' is required.");

            if (domain.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            {
                if (domain.Length > 4)
                {
                    return domain.Substring(4);
                }

                return String.Empty;
            }

            return domain;
        }
    }
}
