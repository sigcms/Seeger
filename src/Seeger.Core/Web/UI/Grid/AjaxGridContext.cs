using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.UI.Grid
{
    public class AjaxGridContext
    {
        private string _pageRawUrl;

        public string GridId { get; set; }

        public int PageIndex { get; set; }

        public string PageRawUrl
        {
            get
            {
                return _pageRawUrl;
            }
            set
            {
                if (value != _pageRawUrl)
                {
                    _pageRawUrl = value;
                    QueryString = ParseQueryString(value);
                }
            }
        }

        public NameValueCollection QueryString { get; private set; }

        public AjaxGridContext()
        {
            QueryString = new NameValueCollection();
        }

        public string GetGridControlVirtualPath(string requestedAspxVirtualPath)
        {
            var directory = VirtualPathUtility.GetDirectory(requestedAspxVirtualPath);
            var pageFileName = Path.GetFileNameWithoutExtension(requestedAspxVirtualPath);

            var gridControlFileName = pageFileName + "_" + (String.IsNullOrEmpty(GridId) ? "Grid" : GridId) + ".ascx";

            return VirtualPathUtility.Combine(directory, gridControlFileName);
        }

        private NameValueCollection ParseQueryString(string pageRawUrl)
        {
            var nv = new NameValueCollection();

            if (String.IsNullOrEmpty(pageRawUrl))
            {
                return nv;
            }

            pageRawUrl = (pageRawUrl ?? String.Empty).Trim().Trim('?');

            var indexOfAsk = pageRawUrl.IndexOf('?');

            if (indexOfAsk < 0 || indexOfAsk == pageRawUrl.Length - 1)
            {
                return nv;
            }

            pageRawUrl = pageRawUrl.Substring(indexOfAsk + 1);

            if (!String.IsNullOrEmpty(pageRawUrl))
            {
                pageRawUrl = HttpUtility.UrlDecode(pageRawUrl);

                foreach (var each in pageRawUrl.Split('&'))
                {
                    var parts = each.Split('=');
                    nv[parts[0]] = parts[1];
                }
            }
            
            return nv;
        }
    }

    public class AjaxGridContext<TSearchModel> : AjaxGridContext
    {
        public TSearchModel SearchModel { get; set; }
    }
}
