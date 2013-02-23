using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Seeger.Web.UI.DataManagement
{
    public class EntityKeyCache
    {
        private static readonly Dictionary<Type, string> _keyDic = new Dictionary<Type, string>();
        private static readonly HashSet<Type> _noKeyTypes = new HashSet<Type>();

        public static string GetEntityKeyPropertyName(Type entityType)
        {
            Require.NotNull(entityType, "entityType");

            string name = null;

            if (_keyDic.TryGetValue(entityType, out name))
            {
                return name;
            }
            if (_noKeyTypes.Contains(entityType))
            {
                return null;
            }

            foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(entityType))
            {
                foreach (Attribute attr in p.Attributes)
                {
                    if (attr is EntityKeyAttribute)
                    {
                        _keyDic.Add(entityType, p.Name);
                        return p.Name;
                    }
                }
            }

            _noKeyTypes.Add(entityType);

            return null;
        }
    }
}
