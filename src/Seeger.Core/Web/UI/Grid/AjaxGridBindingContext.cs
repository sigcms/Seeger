using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.UI.Grid
{
    public class AjaxGridBindingContext
    {
        public string GridId { get; set; }

        public int PageIndex { get; set; }

        public string PageRawUrl { get; set; }

        public NameValueCollection QueryString { get; set; }

        public AjaxGridBindingContext(string gridId, int pageIndex, string pageRawUrl)
        {
            Require.NotNullOrEmpty(pageRawUrl, "pageRawUrl");

            GridId = gridId;
            PageIndex = pageIndex;
            PageRawUrl = pageRawUrl;
            QueryString = ParseQueryString(pageRawUrl);
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

    public class AjaxGridBindingContext<TSearchModel> : AjaxGridBindingContext
    {
        public TSearchModel SearchModel { get; set; }

        public AjaxGridBindingContext(TSearchModel searchModel, string gridId, int pageIndex, string pageRawUrl)
            : base(gridId, pageIndex, pageRawUrl)
        {
            SearchModel = searchModel;
        }
    }
}
