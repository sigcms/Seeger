using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public class SerializableCollection<T> : ISerializableCollection<T>
    {
        private HashSet<T> _innerHashSet = new HashSet<T>();

        public bool IsReadOnly
        {
            get { return false; }
        }

        public string Separator { get; private set; }

        public int Count
        {
            get { return _innerHashSet.Count; }
        }

        public SerializableCollection(string separator)
        {
            Require.NotNullOrEmpty(separator, "separator");

            Separator = separator;
        }

        public SerializableCollection(string separator, IEnumerable<T> collection)
        {
            Require.NotNullOrEmpty(separator, "separator");

            Separator = separator;

            if (collection != null)
            {
                foreach (var item in collection)
                {
                    if (!_innerHashSet.Contains(item))
                    {
                        _innerHashSet.Add(item);
                    }
                }
            }
        }

        public SerializableCollection(string separator, string serializedString)
            : this(separator, serializedString, null)
        {
        }

        public SerializableCollection(string separator, string serializedString, Func<string, T> itemParser)
        {
            Require.NotNullOrEmpty(separator, "separator");

            Separator = separator;

            if (!String.IsNullOrEmpty(serializedString))
            {
                var items = serializedString.Split(new String[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(it =>
                                  {
                                      if (itemParser != null)
                                      {
                                          return itemParser.Invoke(it);
                                      }

                                      return (T)Convert.ChangeType(it.Trim(), typeof(T));
                                  });

                foreach (var each in items)
                {
                    if (!_innerHashSet.Contains(each))
                    {
                        _innerHashSet.Add(each);
                    }
                }
            }
        }

        public virtual void Add(T item)
        {
            _innerHashSet.Add(item);
        }

        public virtual void Remove(T item)
        {
            _innerHashSet.Remove(item);
        }

        public virtual void Clear()
        {
            foreach (var item in _innerHashSet.ToList())
            {
                Remove(item);
            }
        }

        public bool Contains(T item)
        {
            return _innerHashSet.Contains(item);
        }

        public string Serialize()
        {
            return Serialize(null);
        }

        public string Serialize(Func<T, string> itemSerailzer)
        {
            StringBuilder builder = new StringBuilder();

            bool first = true;

            foreach (var item in _innerHashSet)
            {
                if (!first)
                {
                    builder.Append(Separator);
                }

                if (itemSerailzer != null)
                {
                    builder.Append(itemSerailzer.Invoke(item));
                }
                else
                {
                    builder.Append(item.AsString());
                }

                first = false;
            }

            return builder.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerHashSet.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Serialize();
        }
    }
}
