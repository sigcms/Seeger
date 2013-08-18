using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Sample.Domain
{
    public class BlogPost
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Content { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public BlogPost()
        {
            CreatedAt = DateTime.Now;
        }
    }
}