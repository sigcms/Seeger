using Seeger.Files;
using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Files
{
    public partial class Buckets_Grid : AjaxGridUserControlBase
    {
        protected IList<FileBucketMeta> Metas { get; set; }

        public override void Bind(AjaxGridContext context)
        {
            var store = FileBucketMetaStores.Current;
            Metas = store.LoadAll();
        }

        protected string GetConfigurationUrl(string bucketId)
        {
            var meta = FileBucketMetaStores.Current.Load(bucketId);
            var provider = FileSystemProviders.Get(meta.FileSystemProviderName);
            return provider.GetConfigurationUrl(bucketId);
        }
    }
}