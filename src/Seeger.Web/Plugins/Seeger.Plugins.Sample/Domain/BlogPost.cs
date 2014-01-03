using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seeger.Data.Mapping;
using Seeger.ComponentModel;

namespace Seeger.Plugins.Sample.Domain
{
    [Entity]
    public class BlogPost
    {
        [HiloId]
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        [StringClob]
        public virtual string Content { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public BlogPost()
        {
            CreatedAt = DateTime.Now;
        }
    }
}