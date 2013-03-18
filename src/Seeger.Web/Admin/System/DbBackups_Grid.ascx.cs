using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin._System
{
    public partial class DbBackups_Grid : AjaxGridUserControlBase
    {
        public override void Bind(AjaxGridContext context)
        {
            var directory = new DirectoryInfo(Server.MapPath("~/App_Data/Backups"));
            if (directory.Exists)
            {
                var files = directory.GetFiles().Where(f => !f.IsHidden()).ToList();
                List.DataSource = files;
                List.DataBind();
            }
        }
    }
}