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
    public partial class List_Grid : AjaxGridUserControlBase
    {
        protected string CurrentPath { get; private set; }

        public override void Bind(AjaxGridContext context)
        {
            CurrentPath = context.QueryString.TryGetValue("path", FileExplorer.AllowedUploadPaths.First());
            List.DataSource = FileExplorer.List(CurrentPath);
            List.DataBind();
        }
    }
}