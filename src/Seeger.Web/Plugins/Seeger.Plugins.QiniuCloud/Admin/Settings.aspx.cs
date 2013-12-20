using Seeger.Files;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.QiniuCloud.Admin
{
    public partial class Settings : AdminPageBase
    {
        protected string BucketId { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            BucketId = Request.QueryString["bucketId"];

            if (!IsPostBack)
            {
                Bind(FileBucketMetaStores.Current.Load(BucketId));
            }
        }

        private void Bind(FileBucketMeta meta)
        {
            QiniuBucket.Text = meta.Config.GetValueOrDefault("Bucket");
            Domain.Text = meta.Config.GetValueOrDefault("Domain");
            AccessKey.Text = meta.Config.GetValueOrDefault("AccessKey");
            SecurityKey.Text = meta.Config.GetValueOrDefault("SecurityKey");
        }

        private void Update(FileBucketMeta meta)
        {
            meta.Config.AddOrSet("Bucket", QiniuBucket.Text.Trim());
            meta.Config.AddOrSet("Domain", Domain.Text.Trim());
            meta.Config.AddOrSet("AccessKey", AccessKey.Text.Trim());
            meta.Config.AddOrSet("SecurityKey", SecurityKey.Text.Trim());
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var bucket = FileBucketMetaStores.Current.Load(BucketId);
            Update(bucket);

            FileBucketMetaStores.Current.Save(bucket);

            Response.Redirect("/Admin/Files/Buckets.aspx");
        }
    }
}