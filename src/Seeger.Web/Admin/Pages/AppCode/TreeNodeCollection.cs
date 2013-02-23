using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Web.UI.Admin.Pages
{
    public class TreeNodeCollection : IEnumerable<TreeNode>
    {
        private TreeNode _parent;
        private List<TreeNode> _innerList = new List<TreeNode>();

        public TreeNodeCollection(TreeNode parent)
        {
            _parent = parent;
        }

        public TreeNode Parent
        {
            get
            {
                return _parent;
            }
        }

        public TreeNode this[int index]
        {
            get
            {
                return _innerList[index];
            }
        }

        public int Count
        {
            get { return _innerList.Count; }
        }

        public void Add(TreeNode node)
        {
            node.Parent = _parent;
            _innerList.Add(node);
        }

        public void Remove(TreeNode node)
        {
            _innerList.Remove(node);
        }

        public IEnumerator<TreeNode> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}