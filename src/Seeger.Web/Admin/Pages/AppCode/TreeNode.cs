using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using Seeger.Caching;
using System.Text;

namespace Seeger.Web.UI.Admin.Pages
{
    public class TreeNode
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Encoded { get; set; }
        public string ImageUrl { get; set; }
        public bool Expanded { get; set; }
        [JsonProperty("Items")]
        public TreeNodeCollection Nodes { get; private set; }
        [JsonIgnore]
        public TreeNode Parent { get; set; }
        
        public TreeNode()
        {
            Nodes = new TreeNodeCollection(this);
        }

        public static TreeNodeCollection FromPageCache(PageCache cache)
        {
            var rootNodes = new TreeNodeCollection(null);
            foreach (var page in cache.RootPages)
            {
                var node = CreateNode(page);
                rootNodes.Add(node);

                AddNode(page, node);
            }

            return rootNodes;
        }

        static void AddNode(PageItem page, TreeNode node)
        {
            foreach (var child in page.Pages)
            {
                var childNode = CreateNode(child);
                node.Nodes.Add(childNode);

                AddNode(child, childNode);
            }
        }

        static TreeNode CreateNode(PageItem page)
        {
            return new TreeNode
            {
                Text = String.Format("<span id='page-{0}' class='node-text'>{1}</span><input class='pageinfo' type='hidden' value='{2}' />", page.Id, page.DisplayName, new PageItemView(page).ToJson().Replace("\"", "&quot;")),
                Value = page.Id.ToString(),
                Encoded = false,
                Expanded = false,
                ImageUrl = GetNodeImageUrl(page)
            };
        }

        public static string GetNodeImageUrl(PageItem page)
        {
            string fileName = page.Published ? "icon-page-online.png" : "icon-page-offline.png";

            return UrlUtil.ToAbsoluteHtmlPath(UrlUtil.Combine(AdminSession.Current.Skin.VirtualPath, "Images/" + fileName));
        }

        public static string RenderHtml(TreeNodeCollection nodes)
        {
            if (nodes.Count == 0)
            {
                return String.Empty;
            }

            StringBuilder html = new StringBuilder();
            html.Append("<ul class='t-group t-treeview-lines'>");

            for (var i = 0; i < nodes.Count; i++)
            {
                RenderNode(html, nodes[i], i, nodes.Count);
            }

            html.Append("</ul>");

            return html.ToString();
        }

        private static void RenderNode(StringBuilder html, TreeNode node, int index, int count)
        {
            html.Append("<li class='t-item");

            if (index == 0)
            {
                html.Append(" t-first");
            }
            if (index == count - 1)
            {
                html.Append(" t-last");
            }

            html.Append("'>");

            RenderNodeDiv(html, node, index, count);

            if (node.Nodes.Count > 0)
            {
                html.Append("<ul class='t-group'");

                if (!node.Expanded)
                {
                    html.Append(" style='display:none'");
                }

                html.Append(">");

                for (var i = 0; i < node.Nodes.Count; i++)
                {
                    RenderNode(html, node.Nodes[i], i, node.Nodes.Count);
                }

                html.Append("</ul>");
            }

            html.Append("</li>");
        }

        private static void RenderNodeDiv(StringBuilder html, TreeNode node, int index, int count)
        {
            html.Append("<div class='");

            if (index == (count - 1))
            {
                html.Append("t-bot");
            }
            else if (index == 0)
            {
                html.Append("t-top");
            }
            else
            {
                html.Append("t-mid");
            }
            html.Append("'>");

            RenderPlusMinusIcon(html, node);

            html.Append("<span class='t-in'>");
            RenderNodeImage(html, node);
            html.Append(node.Text);
            html.Append("</span>");

            RenderNodeValue(html, node);

            html.Append("</div>");
        }

        private static void RenderNodeValue(StringBuilder html, TreeNode node)
        {
            if (!String.IsNullOrEmpty(node.Value))
            {
                html.AppendFormat("<input type='hidden' class='t-input' value='{0}' name='itemValue' />", node.Value);
            }
        }

        private static void RenderNodeImage(StringBuilder html, TreeNode node)
        {
            if (!String.IsNullOrEmpty(node.ImageUrl))
            {
                html.AppendFormat("<img src='{0}' alt='' class='t-image' />", node.ImageUrl);
            }
        }

        private static void RenderPlusMinusIcon(StringBuilder html, TreeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                string cssClass = "t-icon t-plus";
                if (node.Expanded)
                {
                    cssClass = "t-icon t-minus";
                }

                html.AppendFormat("<span class='{0}'></span>", cssClass);
            }
        }
    }
}