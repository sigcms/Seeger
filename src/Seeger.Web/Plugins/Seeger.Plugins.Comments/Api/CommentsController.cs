using Seeger.Data;
using Seeger.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NHibernate.Linq;
using System.Web;

namespace Seeger.Plugins.Comments.Api
{
    [Area("cmt")]
    public class CommentsController : ApiController
    {
        [HttpGet]
        public PagedDataList<Comment> Get(string subjectId, int start = Int32.MaxValue, int limit = 10)
        {
            var session = Database.GetCurrentSession();
            var query = session.Query<Comment>()
                               .Where(c => c.SubjectId == subjectId && c.ParentCommentId == null);

            var total = query.Count();

            if (start != Int32.MaxValue && start > 0)
            {
                query = query.Where(c => c.Id <= start);
            }

            var items = query.OrderByDescending(c => c.Id)
                             .Take(limit)
                             .ToList();

            return new PagedDataList<Comment>
            {
                Items = items,
                TotalItems = total
            };
        }

        [HttpGet, ActionName("Replies")]
        public List<PagedDataList<Comment>> GetReplies(string commentIds, int start = 0, int limit = 5)
        {
            var result = new List<PagedDataList<Comment>>();
            var commentIdArray = commentIds.Split(',').Select(it => Convert.ToInt32(it)).ToArray();

            foreach (var commentId in commentIdArray)
            {
                var session = Database.GetCurrentSession();
                var query = session.Query<Comment>()
                                   .Where(c => c.ParentCommentId == commentId);

                var total = query.Count();
                var items = query.Where(c => c.Id >= start)
                                 .OrderBy(c => c.Id)
                                 .Take(limit)
                                 .ToList();

                result.Add(new PagedDataList<Comment>
                {
                    Items = items,
                    TotalItems = total
                });
            }

            return result;
        }

        [HttpPost, CommentAuthorize]
        public Comment Post(Comment model)
        {
            var session = Database.GetCurrentSession();

            var parent = model.ParentCommentId == null ? null : session.Get<Comment>(model.ParentCommentId.Value);
            var comment = new Comment
            {
                SubjectId = model.SubjectId,
                SubjectTitle = model.SubjectTitle,
                Content = HttpUtility.HtmlEncode(model.Content),
                ParentCommentId = parent == null ? null : (int?)parent.Id
            };

            var commenter = AuthenticationContexts.Current.GetCurrentUser(new HttpContextWrapper(HttpContext.Current));

            comment.CommenterId = commenter.Id;
            comment.CommenterNick = commenter.Nick;
            comment.CommenterAvatar = commenter.Avatar;
            comment.CommenterIP = HttpContext.Current.Request.GetIPAddress();

            session.Save(comment);

            if (parent != null)
            {
                parent.TotalReplies++;
            }

            session.Commit();

            return comment;
        }
    }
}