using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Seeger.Web
{
    public class UrlUtility
    {
        public static string RemoveFirstSegment(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return String.Empty;
            }

            return RemoveFirstSegment(path, path.StartsWith("/"));
        }

        public static string RemoveFirstSegment(string path, bool headingSlash)
        {
            string firstSegment = GetFirstSegment(path);

            if (String.IsNullOrEmpty(firstSegment))
            {
                return headingSlash ? "/" : String.Empty;
            }

            int start = path.StartsWith("/") ? firstSegment.Length + 1 : firstSegment.Length;

            if (!headingSlash)
            {
                ++start;
            }

            if (start > path.Length - 1)
            {
                return headingSlash ? "/" : String.Empty;
            }

            return path.Substring(start);
        }

        public static string GetFirstSegment(string path)
        {
            if (String.IsNullOrEmpty(path) || path == "/")
            {
                return String.Empty;
            }

            int start = 0;
            if (path.StartsWith("/"))
            {
                start = 1;
            }

            int end = path.IndexOf('/', start);

            if (end < 0)
            {
                return path.Substring(start);
            }

            return path.Substring(start, end - start);
        }

        public static string ToAbsoluteHtmlPath(string path)
        {
            if (path.StartsWith("/"))
            {
                return path;
            }
            if (path.StartsWith("~/"))
            {
                return path.Substring(1);
            }
            return "/" + path;
        }

        public static string GetParentPath(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return String.Empty;
            }

            if (path.Length == 1)
            {
                return "/";
            }

            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }

            int index = path.LastIndexOf('/');

            if (index == 0)
            {
                return "/";
            }

            return path.Substring(0, index);
        }

        public static string Combine(params string[] paths)
        {
            string path = String.Empty;

            IEnumerator enumerator = paths.GetEnumerator();
            if (enumerator.MoveNext())
            {
                path = (String)enumerator.Current;
            }

            while (enumerator.MoveNext())
            {
                path = Combine(path, (String)enumerator.Current);
            }

            return path;
        }

        public static string Combine(string path1, string path2)
        {
            if (String.IsNullOrEmpty(path1))
            {
                return path2;
            }
            if (String.IsNullOrEmpty(path2))
            {
                return path1;
            }

            string result = null;

            if (!path1.EndsWith("/"))
            {
                path1 = path1 + "/";
            }
            if (path2.StartsWith("/"))
            {
                if (path2.Length == 1)
                {
                    result = path1;
                }
                else
                {
                    result = String.Concat(path1, path2.Substring(1));
                }
            }
            else
            {
                result = String.Concat(path1, path2);
            }

            return result;
        }
    }
}
