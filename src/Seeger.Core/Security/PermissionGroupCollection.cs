using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Security
{
    public class PermissionGroupCollection : IEnumerable<PermissionGroup>
    {
        Dictionary<string, PermissionGroup> _innerDic = new Dictionary<string, PermissionGroup>(StringComparer.OrdinalIgnoreCase);

        public PermissionGroupCollection()
        {
        }

        public int Count
        {
            get { return _innerDic.Count; }
        }

        public bool Contains(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            return _innerDic.ContainsKey(name);
        }

        public PermissionGroup Find(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            PermissionGroup fun = null;
            _innerDic.TryGetValue(name, out fun);

            return fun;
        }

        public void Add(PermissionGroup fun)
        {
            Require.NotNull(fun, "fun");
            
            _innerDic.Add(fun.Name, fun);
        }

        public IEnumerator<PermissionGroup> GetEnumerator()
        {
            return _innerDic.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
