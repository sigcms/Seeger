using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public static class TreeNodeExtensions
    {
        public static IList<T> PathFromRoot<T>(this T node, bool includeSelf)
            where T : class, ITreeNode<T>
        {
            return PathToRoot(node, includeSelf).Reverse().ToList();
        }

        public static IList<T> PathToRoot<T>(this T node, bool includeSelf)
            where T : class, ITreeNode<T>
        {
            Require.NotNull(node, "node");

            var path = new List<T>();
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

        public static IEnumerable<T> Parents<T>(this T node, Func<T, bool> predicate)
            where T : class, ITreeNode<T>
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
            where T : class, ITreeNode<T>
        {
            Require.NotNull(node, "node");

            if (node.Parent == null)
            {
                return Enumerable.Empty<T>();
            }

            return node.Parent.Children.Where(it => !Object.ReferenceEquals(it, node) && predicate(it));
        }

        public static T FindDescendant<T>(this T node, Func<T, bool> predicate)
            where T : class, ITreeNode<T>
        {
            Require.NotNull(node, "node");
            Require.NotNull(predicate, "predicate");

            foreach (var child in node.Children)
            {
                if (predicate(child))
                {
                    return child;
                }

                var result = child.FindDescendant(predicate);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static IEnumerable<T> FindAllDescendants<T>(this T node, Func<T, bool> predicate)
            where T : class, ITreeNode<T>
        {
            Require.NotNull(node, "node");
            Require.NotNull(predicate, "predicate");

            var result = new List<T>();

            foreach (var child in node.Children)
            {
                if (predicate(child))
                {
                    result.Add(child);
                }

                result.AddRange(child.FindAllDescendants(predicate));
            }

            return result;
        }
    }

}
