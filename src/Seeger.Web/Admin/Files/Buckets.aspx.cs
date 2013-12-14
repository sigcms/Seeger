using Seeger.Files;
using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Files
{
    public partial class Buckets : AjaxGridPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod, ScriptMethod]
        public static void SetDefault(string bucketId)
        {
            new FileBucketService(FileBucketMetaStores.Current).SetDefault(bucketId);
        }
    }
}