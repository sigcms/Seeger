using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public interface ITreeNodeVisitor<T>
        where T : ITreeNode<T>
    {
        bool Visit(T node);
    }

    internal class TreeNodeFuncVisitor<T> : ITreeNodeVisitor<T>
        where T : ITreeNode<T>
    {
        private Func<T, bool> _func;

        public TreeNodeFuncVisitor(Func<T, bool> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            _func = func;
        }

        #region ITreeNodeVisitor<T> Members

        public bool Visit(T node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            return _func(node);
        }

        #endregion
    }
}
