using Seeger.Files;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Files
{
    public partial class LocalFileSystemConfig : AdminPageBase
    {
        protected string BucketId
        {
            get
            {
                return Request.QueryString["bucketId"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind(FileBucketMetaStores.Current.Load(BucketId));
            }
        }

        private void Bind(FileBucketMeta meta)
        {
            BaseVirtualPath.Text = UrlUtil.RemoveFirstSegment(meta.Config.GetValueOrDefault("BaseVirtualPath"));
        }

        private void Update(FileBucketMeta meta)
        {
            meta.Config.AddOrSet("BaseVirtualPath", UrlUtil.Combine("/Files/", BaseVirtualPath.Text.Trim()));
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var meta = FileBucketMetaStores.Current.Load(BucketId);
            Update(meta);
            FileBucketMetaStores.Current.Save(meta);

            Response.Redirect("Buckets.aspx");
        }
    }
}