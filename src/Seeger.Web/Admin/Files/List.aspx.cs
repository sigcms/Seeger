using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Files;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Files
{
    public partial class List : AdminPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "FileMgnt", "View");
        }

        protected string CurrentPath
        {
            get
            {
                string path = Request.QueryString["path"];
                if (String.IsNullOrEmpty(path))
                {
                    path = FileExplorer.AllowedUploadPaths.First();
                }
                else if (!FileExplorer.AllowUploadPath(path))
                {
                    throw new InvalidOperationException("Path denied: " + path);
                }

                return path.TrimEnd('/');
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindToolbar();
                BindList(CurrentPath);
            }
        }

        private void BindToolbar()
        {
            string[] paths = CurrentPath.SplitWithoutEmptyEntries('/');
            if (paths.Length >= 2)
            {
                UpButton.Enabled = true;
                UpButton.OnClientClick = String.Format("location.href='?path=/{0}';return false;", String.Join("/", paths, 0, paths.Length - 1));
            }

            UploadFileButton.OnClientClick = "location.href='Upload.aspx?path=" + CurrentPath + "';return false;";
        }

        private void BindList(string path)
        {
            var items = FileExplorer.List(path);
            Grid.VirtualItemsCount = items.Count;
            Grid.DataSource = items;
            Grid.DataBind();
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                string path = Server.MapPath(UrlUtility.Combine(CurrentPath, e.CommandArgument.ToString()));
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                BindList(CurrentPath);
            }
        }

        protected void Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                InitFileNameCell(e.Row);
                InitRenameButton(e.Row);
                InitDeleteButton(e.Row);
            }
        }

        private void InitRenameButton(GridViewRow row)
        {
            var button = (AdminLinkButton)row.Cells[row.Cells.Count - 1].FindControl("RenameButton");
            button.Text = "[" + button.Text + "]";

            if (((FileSystemEntry)row.DataItem).IsDirectory)
            {
                button.Permission = "RenameFolder";
            }
            else
            {
                button.Permission = "RenameFile";
            }
        }

        private void InitDeleteButton(GridViewRow row)
        {
            var button = (AdminLinkButton)row.Cells[row.Cells.Count - 1].FindControl("DeleteButton");
            button.ToolTip = Localize("Common.Delete");
            button.Text = "[" + button.Text + "]";
            button.OnClientClick = String.Format("if (!confirm('{0}')) return false;", Localize("Message.DeleteConfirm"));

            if (((FileSystemEntry)row.DataItem).IsDirectory)
            {
                button.Permission = "DeleteFolder";
            }
            else
            {
                button.Permission = "DeleteFile";
            }
        }

        private static void InitFileNameCell(GridViewRow row)
        {
            FileSystemEntry entry = (FileSystemEntry)row.DataItem;

            HyperLink link = (HyperLink)row.Cells[0].FindControl("ItemLink");
            link.Text = entry.Name;
            link.Attributes["isdirectory"] = entry.IsDirectory ? "1" : "0";

            if (entry.Extension != null && entry.Extension.Length > 1)
            {
                link.CssClass += " icon-file icon-" + entry.Extension.Substring(1).ToLower();
            }
            else
            {
                link.CssClass += " icon-folder";
            }

            if (entry.IsDirectory)
            {
                link.NavigateUrl = "?path=" + entry.VirtualPath;
            }
            else
            {
                link.NavigateUrl = entry.VirtualPath;
                link.Target = "_blank";
            }
        }
    }
}