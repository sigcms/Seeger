using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public class ReadOnlySerializableCollection<T> : ISerializableCollection<T>
    {
        private SerializableCollection<T> _innerCollection;

        bool ISerializableCollection<T>.IsReadOnly
        {
            get { return true; }
        }

        public string Separator
        {
            get
            {
                return _innerCollection.Separator;
            }
        }

        public int Count
        {
            get
            {
                return _innerCollection.Count;
            }
        }

        public ReadOnlySerializableCollection(string separator, IEnumerable<T> collection)
        {
            _innerCollection = new SerializableCollection<T>(separator, collection);
        }

        private ReadOnlySerializableCollection(SerializableCollection<T> innerCollection)
        {
            _innerCollection = innerCollection;
        }

        public static ReadOnlySerializableCollection<T> Parse(string separator, string serializedString)
        {
            return new ReadOnlySerializableCollection<T>(new SerializableCollection<T>(separator, serializedString));
        }

        public static ReadOnlySerializableCollection<T> Parse(string separator, string serializedString, Func<string, T> itemParser)
        {
            return new ReadOnlySerializableCollection<T>(new SerializableCollection<T>(separator, serializedString));
        }

        public bool Contains(T item)
        {
            return _innerCollection.Contains(item);
        }

        public string Serialize()
        {
            return _innerCollection.Serialize();
        }

        public string Serialize(Func<T, string> itemSerializer)
        {
            return _innerCollection.Serialize(itemSerializer);
        }

        void ISerializableCollection<T>.Add(T item)
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        void ISerializableCollection<T>.Remove(T item)
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        void ISerializableCollection<T>.Clear()
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
