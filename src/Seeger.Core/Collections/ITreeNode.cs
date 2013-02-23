using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public interface ITreeNode<T>
        where T : ITreeNode<T>
    {
        T Parent { get; }
        IEnumerable<T> Children { get; }
    }
}
