using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.UI
{
    public class PageResources
    {
        private List<StyleResource> _styles;

        public IList<StyleResource> Styles
        {
            get
            {
                if (_styles == null)
                {
                    _styles = new List<StyleResource>();
                }
                return _styles;
            }
        }

        private List<ScriptResource> _headScripts;

        public IList<ScriptResource> HeadScripts
        {
            get
            {
                if (_headScripts == null)
                {
                    _headScripts = new List<ScriptResource>();
                }
                return _headScripts;
            }
        }

        private List<ScriptResource> _footScripts;

        public IList<ScriptResource> FootScripts
        {
            get
            {
                if (_footScripts == null)
                {
                    _footScripts = new List<ScriptResource>();
                }
                return _footScripts;
            }
        }

        public static PageResources GetCurrent()
        {
            return GetCurrent(HttpContext.Current);
        }

        public static PageResources GetCurrent(HttpContext httpContext)
        {
            const string key = "PageResources.Current";
            var resources = httpContext.Items[key] as PageResources;
            if (resources == null)
            {
                resources = new PageResources();
                httpContext.Items.Add(key, resources);
            }

            return resources;
        }

        public IHtmlString RenderStyles()
        {
            if (_styles == null)
            {
                return new HtmlString(String.Empty);
            }

            var html = new StringBuilder();
            foreach (var style in _styles)
            {
                html.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\"{1} />", style.Path, String.IsNullOrEmpty(style.Media) ? String.Empty : " media=\"" + style.Media + "\"");
            }

            return new HtmlString(html.ToString());
        }

        public IHtmlString RenderHeadScripts()
        {
            return RenderScripts(_headScripts);
        }

        public IHtmlString RenderFootScripts()
        {
            return RenderScripts(_footScripts);
        }

        private IHtmlString RenderScripts(IEnumerable<ScriptResource> scripts)
        {
            if (scripts == null)
            {
                return new HtmlString(String.Empty);
            }

            var html = new StringBuilder();
            foreach (var script in scripts)
            {
                html.AppendFormat("<script src=\"{0}\"></script>", script.Path).AppendLine();
            }

            return new HtmlString(html.ToString());
        }
    }
}
