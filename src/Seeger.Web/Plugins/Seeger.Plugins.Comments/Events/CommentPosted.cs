using Seeger.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments.Events
{
    public class CommentPosted : IEvent
    {
        public Comment Comment { get; set; }

        public CommentPosted(Comment comment)
        {
            Comment = comment;
        }
    }
}