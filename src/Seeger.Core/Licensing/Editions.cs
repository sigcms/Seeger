using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Licensing
{
    public class Editions
    {
        private static readonly Dictionary<string, Edition> _editions;

        static Editions()
        {
            _editions = new Dictionary<string, Edition>(StringComparer.OrdinalIgnoreCase);

            var std = new StandardEdition();
            _editions.Add(std.Name, std);

            var international = new InternationalEdition();
            _editions.Add(international.Name, international);
        }

        public static IEnumerable<Edition> GetAllEditions()
        {
            return new List<Edition>(_editions.Values);
        }

        public static bool Contains(string name)
        {
            return _editions.ContainsKey(name);
        }

        public static Edition GetEdition(string name)
        {
            if (_editions.ContainsKey(name))
            {
                return _editions[name];
            }

            return InvalidEdition.Instance;
        }
    }
}
