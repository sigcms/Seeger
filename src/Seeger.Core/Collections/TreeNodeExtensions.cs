using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public static class TreeNodeExtensions
    {
        #region Path

        public static IList<T> PathFromRoot<T>(this T node, bool includeSelf)
            where T : ITreeNode<T>
        {
            return PathToRoot(node, includeSelf).Reverse().ToList();
        }

        public static IList<T> PathToRoot<T>(this T node, bool includeSelf)
            where T : ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            List<T> path = new List<T>();
            if (includeSelf)
            {
                path.Add(node);
            }

            T current = node;
            while (current.Parent != null)
            {
                path.Add(current.Parent);
                current = current.Parent;
            }

            return path;
        }

        #endregion

        public static IEnumerable<T> Parents<T>(this T node, Func<T, bool> predicate)
            where T : ITreeNode<T>
        {
            Require.NotNull(node, "node");

            var temp = node.Parent;

            while (temp != null)
            {
                if (predicate != null)
                {
                    if (predicate(temp))
                    {
                        yield return temp;
                    }
                }
                else
                {
                    yield return temp;
                }

                temp = temp.Parent;
            }
        }

        public static IEnumerable<T> Siblings<T>(this T node, Func<T, bool> predicate)
            where T : ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (node.Parent == null)
            {
                return Enumerable.Empty<T>();
            }
            return node.Parent.Children.Where(it => !Object.ReferenceEquals(it, node) && predicate(it));
        }

        #region Depth-First Visit / Search

        public static T DepthFirstSearch<T>(this T node, bool includeSelf, Func<T, bool> predicate)
            where T : ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            T result = default(T);

            node.DepthFirstVisit(includeSelf, it =>
            {
                if (predicate(it))
                {
                    result = it;
                    return true;
                }
                return false;

            });

            return result;
        }

        public static IEnumerable<T> DepthFirstSearchAll<T>(this T node, bool includeSelf, Func<T, bool> predicate)
            where T : ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            List<T> result = new List<T>();

            node.DepthFirstVisit(includeSelf, it =>
            {
                if (predicate(it))
                {
                    result.Add(it);
                }
                return false;
            });

            return result;
        }

        public static void DepthFirstVisit<T>(this T node, bool includeSelf, Func<T, bool> visitor)
            where T : ITreeNode<T>
        {
            DepthFirstVisit(node, includeSelf, new TreeNodeFuncVisitor<T>(visitor));
        }

        public static void DepthFirstVisit<T>(this T node, bool includeSelf, ITreeNodeVisitor<T> visitor)
            where T : ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }

            Stack<T> stack = new Stack<T>();
            if (includeSelf)
            {
                stack.Push(node);
            }
            else
            {
                foreach (T child in node.Children)
                {
                    stack.Push(child);
                }
            }

            while (stack.Count > 0)
            {
                T current = stack.Pop();

                if (visitor.Visit(current))
                {
                    break;
                }

                foreach (T child in current.Children)
                {
                    stack.Push(child);
                }

            }
        }

        #endregion

        #region Breadth-First Visit / Search

        public static T BreadthFirstSearch<T>(this T node, bool includeSelf, Func<T, bool> predicate)
            where T : ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            T result = default(T);

            node.BreadthFirstVisit(includeSelf, it =>
            {
                if (predicate(it))
                {
                    result = it;
                    return true;
                }
                return false;

            });

            return result;
        }

        public static IEnumerable<T> BreadthFirstSearchAll<T>(this T node, bool includeSelf, Func<T, bool> predicate)
            where T : ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            List<T> result = new List<T>();

            node.BreadthFirstVisit(includeSelf, it =>
            {
                if (predicate(it))
                {
                    result.Add(it);
                }
                return false;
            });

            return result;
        }

        public static void BreadthFirstVisit<T>(this T node, bool includeSelf, Func<T, bool> visitor)
            where T : ITreeNode<T>
        {
            BreadthFirstVisit<T>(node, includeSelf, new TreeNodeFuncVisitor<T>(visitor));
        }

        public static void BreadthFirstVisit<T>(this T node, bool includeSelf, ITreeNodeVisitor<T> visitor)
            where T : ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }

            Queue<T> queue = new Queue<T>();
            if (includeSelf)
            {
                queue.Enqueue(node);
            }
            else
            {
                foreach (T child in node.Children)
                {
                    queue.Enqueue(child);
                }
            }

            while (queue.Count > 0)
            {
                T current = queue.Dequeue();

                if (visitor.Visit(current))
                {
                    break;
                }

                foreach (T child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        #endregion
    }

}
