using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Seeger.Collections;

namespace Seeger.Security
{
    public class PermissionCollection : IEnumerable<Permission>
    {
        Dictionary<string, Permission> _innerDic = new Dictionary<string, Permission>(StringComparer.OrdinalIgnoreCase);

        public PermissionCollection()
        {
        }

        public PermissionCollection(IEnumerable<Permission> operations)
        {
            if (operations != null)
            {
                foreach (var op in operations)
                {
                    _innerDic.Add(op.Name, op);
                }
            }
        }

        public int Count
        {
            get { return _innerDic.Count; }
        }

        public bool Contains(string operation)
        {
            Require.NotNullOrEmpty(operation, "operation");

            return _innerDic.ContainsKey(operation);
        }

        public Permission Find(string operation)
        {
            Require.NotNullOrEmpty(operation, "operation");

            Permission op = null;
            _innerDic.TryGetValue(operation, out op);

            return op;
        }

        public void Add(Permission operation)
        {
            Require.NotNull(operation, "operation");

            if (_innerDic.ContainsKey(operation.Name))
            {
                _innerDic[operation.Name] = operation;
            }
            else
            {
                _innerDic.Add(operation.Name, operation);
            }
        }

        public void Add(IEnumerable<Permission> operations)
        {
            Require.NotNull(operations, "operations");

            foreach (var op in operations)
            {
                Add(op);
            }
        }

        public bool Remove(string operation)
        {
            Require.NotNullOrEmpty(operation, "operation");

            Permission op = Find(operation);

            if (op != null)
            {
                _innerDic.Remove(op.Name);

                return true;
            }

            return false;
        }

        public IEnumerator<Permission> GetEnumerator()
        {
            return _innerDic.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
