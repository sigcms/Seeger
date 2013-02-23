using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public class ObservableSerializableCollection<T> : SerializableCollection<T>
    {
        public event CollectionChangedEventHandler<T> CollectionChanged;

        public ObservableSerializableCollection(string separator)
            : base(separator)
        {
        }

        public ObservableSerializableCollection(string separator, IEnumerable<T> collection)
            : base(separator, collection)
        {
        }

        public ObservableSerializableCollection(string separator, string serializedString)
            : base(separator, serializedString)
        {
        }

        public ObservableSerializableCollection(string separator, string serializedString, Func<string, T> itemParser)
            : base(separator, serializedString, itemParser)
        {
        }

        public override void Add(T item)
        {
            base.Add(item);

            OnCollectionChanged(new CollectionChangeEventArgs<T>(new T[] { item }, ObjectChangeType.Added));
        }

        public override void Remove(T item)
        {
            base.Remove(item);

            OnCollectionChanged(new CollectionChangeEventArgs<T>(new T[] { item }, ObjectChangeType.Removed));
        }

        protected virtual void OnCollectionChanged(CollectionChangeEventArgs<T> e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged.Invoke(this, e);
            }
        }
    }
}
