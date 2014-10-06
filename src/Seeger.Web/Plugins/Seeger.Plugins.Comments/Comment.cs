using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments
{
    [Entity]
    public class Comment
    {
        [HiloId]
        public virtual int Id { get; set; }

        public virtual string SubjectType { get; set; }

        public virtual string SubjectId { get; set; }

        public virtual string SubjectTitle { get; set; }

        public virtual string Content { get; set; }

        public virtual DateTime PostedTimeUtc { get; set; }

        public virtual int TotalReplies { get; set; }

        public virtual int? ParentCommentId { get; set; }

        public virtual string CommenterId { get; set; }

        public virtual string CommenterNick { get; set; }

        public virtual string CommenterAvatar { get; set; }

        public virtual string CommenterIP { get; set; }

        public Comment()
        {
            PostedTimeUtc = DateTime.UtcNow;
        }
    }
}