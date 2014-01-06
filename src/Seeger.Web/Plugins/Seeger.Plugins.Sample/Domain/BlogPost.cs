using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seeger.Data.Mapping;
using NHibernate.Mapping.ByCode.Conformist;
using Seeger.ComponentModel;

namespace Seeger.Plugins.Sample.Domain
{
    [Entity]
    public class BlogPost
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        [StringClob]
        public virtual string Content { get; set; }

        public virtual PostVisibility Visibility { get; set; }

        public virtual int Order { get; set; }

        public virtual UserInfo Author { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public BlogPost()
        {
            CreatedAt = DateTime.Now;
        }
    }

    public enum PostVisibility
    {
        Public = 0,
        Private = 1
    }

    //public class BlogPostMap : ClassMapping<BlogPost>
    //{
    //    public BlogPostMap()
    //    {
    //        Table("sample_" + typeof(BlogPost).Name);
    //        Component(x => x.Author, m =>
    //        {
    //            m.Property(c => c.Id, c => c.Column("Author_Id"));
    //            m.Property(c => c.Name, c => c.Column("Author_Name"));
    //        });
    //    }
    //}

    [Component]
    public class UserInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}