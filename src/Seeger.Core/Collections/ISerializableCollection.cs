using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public interface ISerializableCollection<T> : IEnumerable<T>
    {
        bool IsReadOnly { get; }

        string Separator { get; }

        int Count { get; }

        void Add(T item);

        void Remove(T item);

        void Clear();

        bool Contains(T item);

        string Serialize();

        string Serialize(Func<T, string> itemSerializer);
    }
}
