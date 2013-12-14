using Seeger.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Files
{
    public partial class BucketEdit : AdminPageBase
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
                Prepare();

                if (!String.IsNullOrEmpty(BucketId))
                {
                    Bind(FileBucketMetaStores.Current.Load(BucketId));
                }
            }
        }

        private void Prepare()
        {
            FileSystem.DataSource = FileSystemProviders.Providers.Select(x => x.Name);
            FileSystem.DataBind();
        }

        private void Bind(FileBucketMeta meta)
        {
            DisplayName.Text = meta.DisplayName;
            FileSystem.SelectedValue = meta.FileSystemProviderName;
            FileSystem.Enabled = false;
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            FileBucketMeta meta = null;

            if (!String.IsNullOrEmpty(BucketId))
            {
                meta = FileBucketMetaStores.Current.Load(BucketId);
                meta.DisplayName = DisplayName.Text;
            }
            else
            {
                meta = new FileBucketService(FileBucketMetaStores.Current).CreateBucket(DisplayName.Text, FileSystem.SelectedValue, null);
            }

            var provider = FileSystemProviders.Get(meta.FileSystemProviderName);

            Response.Redirect(provider.GetConfigurationUrl(meta.BucketId));
        }
    }
}