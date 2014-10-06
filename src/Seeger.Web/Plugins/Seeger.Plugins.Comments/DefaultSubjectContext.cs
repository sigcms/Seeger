using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Seeger.Plugins.Comments
{
    public class DefaultSubjectContext : ISubjectContext
    {
        public Subject GetCurrentSubject(PageItem pageItem, HttpContextBase httpContext)
        {
            var subjectId = httpContext.Request.RawUrl;

            if (httpContext.Items["Comments.SubjectId"] != null)
            {
                subjectId = httpContext.Items["Comments.SubjectId"].ToString();
            }

            var subject = new Subject
            {
                Id = subjectId
            };

            var page = httpContext.CurrentHandler as Page;
            if (page != null)
            {
                subject.Title = page.Title;
            }

            if (httpContext.Items["Comments.SubjectTitle"] != null)
            {
                subject.Title = httpContext.Items["Comments.SubjectTitle"].ToString();
            }

            if (httpContext.Items["Comments.SubjectType"] != null)
            {
                subject.Type = httpContext.Items["Comments.SubjectType"].ToString();
            }

            return subject;
        }
    }
}