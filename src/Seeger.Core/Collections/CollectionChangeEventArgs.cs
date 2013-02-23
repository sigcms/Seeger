using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public delegate void CollectionChangedEventHandler<T>(object sender, CollectionChangeEventArgs<T> e);

    public class CollectionChangeEventArgs<T> : EventArgs
    {
        public IEnumerable<T> Items { get; private set; }

        public CollectionChangeEventArgs(IEnumerable<T> items, ObjectChangeType changeType)
        {
            Require.NotNull(items, "items");

            this.Items = items;
            this.ChangeType = changeType;
        }

        public ObjectChangeType ChangeType { get; private set; }
    }

    public enum ObjectChangeType
    {
        Added,
        Removed,
        Modified
    }
}
