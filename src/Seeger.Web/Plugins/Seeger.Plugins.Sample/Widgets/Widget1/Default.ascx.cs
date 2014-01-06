using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using Seeger.Plugins.Sample.Domain;
using System.Text;
using Seeger.ComponentModel;

namespace Seeger.Plugins.Sample.Widgets.Widget1
{
    public partial class Default : WidgetControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var entities = NhSession.Query<EntityWithComponentId>().ToList();

            //NhSession.Save(new EntityWithComponentId
            //{
            //    Id = new Identity { Field1 = 1, Field2 = 4 },
            //    Property1 = "test"
            //});
            //NhSession.Commit();

            //var post = new BlogPost
            //{
            //    Title = "title",
            //    Content = "content",
            //    Visibility = PostVisibility.Private,
            //    Order = 5,
            //    Author = new UserInfo
            //    {
            //        Id = 1,
            //        Name = "Admin"
            //    }
            //};

            //NhSession.Save(post);
            //NhSession.Commit();
        }
    }
}