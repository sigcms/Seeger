using NHibernate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Processors
{
    public class HttpProcessingContext
    {
        public ISession NhSession { get; private set; }

        public HttpRequestBase Request { get; private set; }

        public HttpResponseBase Response
        {
            get
            {
                return HttpContext.Response;
            }
        }

        public HttpContextBase HttpContext
        {
            get
            {
                return Request.RequestContext.HttpContext;
            }
        }

        public string FileExtension { get; private set; }

        public IList<string> RemainingSegments { get; private set; }

        public PageItem MatchedPage { get; set; }

        public CultureInfo Culture { get; set; }

        public HttpRedirection Redirection { get; set; }

        public bool StopProcessing { get; set; }

        public HttpProcessingContext(HttpRequestBase request, ISession session)
        {
            Require.NotNull(request, "request");
            Require.NotNull(session, "session");

            Request = request;
            NhSession = session;
            Culture = CultureInfo.CurrentCulture;
            RemainingSegments = request.Path.SplitWithoutEmptyEntries('/').ToList();

            if (!String.IsNullOrEmpty(request.Path))
            {
                FileExtension = Path.GetExtension(request.Path);
            }
            else
            {
                FileExtension = String.Empty;
            }
        }
    }
}
