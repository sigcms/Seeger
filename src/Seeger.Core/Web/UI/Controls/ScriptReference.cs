using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Collections.Specialized;

namespace Seeger.Web.UI
{
    public class ScriptReference : Control
    {
        public string Path
        {
            get
            {
                return ViewState["Path"] as String ?? String.Empty;
            }
            set
            {
                ViewState["Path"] = value;
            }
        }

        public PathMode PathMode
        {
            get
            {
                return ViewState.TryGetValue<PathMode>("PathMode", PathMode.RelativeToCmsRoot);
            }
            set
            {
                ViewState["PathMode"] = value;
            }
        }

        public bool AllowDuplicate
        {
            get
            {
                return ViewState.TryGetValue<bool>("AllowDuplicate", false);
            }
            set
            {
                ViewState["AllowDuplicate"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string url = Path;

            if (PathMode == PathMode.RelativeToCmsRoot)
            {
                url = Path;
            }

            bool render = true;

            if (!AllowDuplicate)
            {
                var registeredScripts = HttpContext.Current.Items["Seeger.Scripts"] as HashSet<string>;

                if (registeredScripts == null)
                {
                    registeredScripts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    HttpContext.Current.Items["Seeger.Scripts"] = registeredScripts;
                }

                if (registeredScripts.Contains(url))
                {
                    render = false;
                }
                else
                {
                    registeredScripts.Add(url);
                }
            }

            if (render)
            {
                writer.Write("<script type=\"text/javascript\" src=\"" + url + "\"></script>");
            }
        }
    }

    public enum PathMode
    {
        RelativeToCmsRoot = 0,
        Normal = 1
    }
}
