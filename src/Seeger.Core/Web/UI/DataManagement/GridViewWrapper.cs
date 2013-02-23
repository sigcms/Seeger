using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seeger.Data;
using Seeger.Globalization;
using NHibernate;
using NHibernate.Linq;

namespace Seeger.Web.UI.DataManagement
{
    public class GridViewWrapper<TEntity>
        where TEntity : class
    {
        public GridViewWrapper(Seeger.Web.UI.GridView grid, ISession session)
        {
            Require.NotNull(grid, "grid");
            Require.NotNull(session, "session");

            this.GridView = grid;
            this.Session = session;

            this.PagerCssClass = "pager";
        }

        public string EntityKeyPropertyName { get; set; }

        private IRecordFilter<TEntity> _recordFilter = new NullRecordFilter<TEntity>();
        private IGridViewCallback<TEntity> _callback = new NullGridViewCallback<TEntity>();

        public IRecordFilter<TEntity> RecordFilter
        {
            get { return _recordFilter; }
            set
            {
                if (value == null)
                {
                    _recordFilter = new NullRecordFilter<TEntity>();
                }
                else
                {
                    _recordFilter = value;
                }
            }
        }

        public IGridViewCallback<TEntity> Callback
        {
            get { return _callback; }
            set
            {
                if (value == null)
                {
                    _callback = new NullGridViewCallback<TEntity>();
                }
                else
                {
                    _callback = value;
                }
            }
        }

        public ISession Session { get; private set; }

        public Func<IQueryable<TEntity>, IQueryable<TEntity>> DataSourceFilter { get; set; }

        public Seeger.Web.UI.GridView GridView { get; private set; }

        public bool AllowPaging
        {
            get { return GridView.AllowPaging; }
            set { GridView.AllowPaging = true; }
        }
        public bool AllowAddition { get; set; }
        public bool AllowDeletion { get; set; }
        public bool AllowEditing { get; set; }

        public int PageIndex
        {
            get { return GridView.PageIndex; }
            set { GridView.PageIndex = value; }
        }

        public int PageSize
        {
            get { return GridView.PageSize; }
            set { GridView.PageSize = value; }
        }

        public string PagerCssClass
        {
            get { return GridView.PagerCssClass; }
            set { GridView.PagerCssClass = value; }
        }

        public string PagerUrlFormat
        {
            get { return GridView.PagerUrlFormat; }
            set { GridView.PagerUrlFormat = value; }
        }

        public string EditingPageUrl { get; set; }

        public void DataBind()
        {
            InitializeGridView();
            GridView.DataBind();
        }

        private bool _gridViewInited;

        protected virtual void InitializeGridView()
        {
            if (!_gridViewInited)
            {
                GridView.AutoGenerateColumns = false;
                GridView.GridLines = GridLines.None;
                if (GridView.CssClass.Length == 0)
                {
                    GridView.CssClass = "datatable";
                }

                GridView.EmptyDataRowStyle.CssClass = "no-record";
                GridView.EmptyDataText = ResourcesFolder.Global.GetValue("Message.NoRecordToDisplay", CultureInfo.CurrentUICulture);

                SetupDataKeyNames();
                BindEditingColumn();
                BindDeletingColumn();

                GridView.RowDataBound += new GridViewRowEventHandler(GridView_RowDataBound);
                GridView.NeedDataSource += new NeedDataSourceEventHandler(GridView_NeedDataSource);

                _gridViewInited = true;
            }
        }

        private void SetupDataKeyNames()
        {
            string keyPropertyName = EntityKeyPropertyName;
            if (String.IsNullOrEmpty(keyPropertyName))
            {
                keyPropertyName = EntityKeyCache.GetEntityKeyPropertyName(typeof(TEntity));

                if (keyPropertyName == null)
                {
                    throw new InvalidOperationException(String.Format("Must add attribute '{0}' to the key property of entity '{1}'.", typeof(EntityKeyAttribute), typeof(TEntity)));
                }
            }

            GridView.DataKeyNames = new string[] { keyPropertyName };
        }

        private void BindEditingColumn()
        {
            if (!AllowEditing) return;

            TemplateField column = new TemplateField();
            GridView.Columns.Add(column);
            column.HeaderText = ResourcesFolder.Global.GetValue("Common.Edit", CultureInfo.CurrentUICulture);
            column.HeaderStyle.CssClass = "edit-column";
            column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            column.ItemTemplate = new EditColumnTemplate
            {
                EditButtonID = "EditButton",
                ImageUrl = UrlUtility.Combine(AdministrationSession.Current.Theme.VirtualPath, "Images/icon-edit.png")
            };
        }

        private void BindDeletingColumn()
        {
            if (!AllowDeletion) return;

            GridView.RowDeleting += new GridViewDeleteEventHandler(GridView_RowDeleting);

            TemplateField column = new TemplateField();
            GridView.Columns.Add(column);
            column.HeaderText = ResourcesFolder.Global.GetValue("Common.Delete", CultureInfo.CurrentUICulture);
            column.HeaderStyle.CssClass = "delete-column";
            column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            column.ItemTemplate = new DeleteColumnTemplate
            {
                ConfirmText = ResourcesFolder.Global.GetValue("Message.DeleteConfirm", CultureInfo.CurrentUICulture),
                ImageUrl = UrlUtility.Combine(AdministrationSession.Current.Theme.VirtualPath, "Images/icon-delete.png")
            };
        }

        private void GridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            object key = GridView.DataKeys[e.RowIndex][EntityKeyPropertyName];
            TEntity entity = Session.Get<TEntity>(key);
            Session.Delete(entity);

            Session.Commit();

            Callback.OnItemDeleted(entity);

            GridView.DataBind();
        }

        private void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (AllowEditing)
                {
                    if (RecordFilter.IsEditable((TEntity)e.Row.DataItem))
                    {
                        TEntity entity = (TEntity)e.Row.DataItem;

                        HyperLink link = (HyperLink)e.Row.FindControl("EditButton");

                        string url = EditingPageUrl;
                        if (url.Contains("?"))
                        {
                            url += "&id=";
                        }
                        else
                        {
                            url += "?id=";
                        }
                        url += ObjectHelper.GetProperty(entity, EntityKeyPropertyName).ToString();

                        link.NavigateUrl = url;
                    }
                    else
                    {
                        e.Row.Cells[e.Row.Cells.Count - (AllowDeletion ? 2 : 1)].Controls.Clear();
                    }
                }

                if (AllowDeletion && !RecordFilter.IsDeletable((TEntity)e.Row.DataItem))
                {
                    e.Row.Cells[e.Row.Cells.Count - 1].Controls.Clear();
                }
            }
        }

        private void GridView_NeedDataSource(object sender, EventArgs e)
        {
            GridView.DataSource = GetDataSource();
        }

        private object GetDataSource()
        {
            IQueryable<TEntity> data = Filter(Session.Query<TEntity>());

            if (AllowPaging)
            {
                GridView.VirtualItemsCount = data.Count();

                data = data.Skip(PageSize * (GridView.PageIndex)).Take(PageSize);
            }

            return data.ToList(); ;
        }

        private IQueryable<TEntity> Filter(IQueryable<TEntity> src)
        {
            if (DataSourceFilter != null)
            {
                return DataSourceFilter.Invoke(src);
            }
            return src;
        }

        #region ColumnTemplate

        private class DeleteColumnTemplate : ITemplate
        {
            public string ConfirmText { get; set; }
            public string ImageUrl { get; set; }

            public void InstantiateIn(Control container)
            {
                ImageButton button = new ImageButton();
                container.Controls.Add(button);
                button.CommandName = "Delete";
                button.ImageUrl = ImageUrl;

                if (!String.IsNullOrEmpty(ConfirmText))
                {
                    button.OnClientClick = String.Format("return confirm('{0}');", ConfirmText);
                }
            }
        }

        private class EditColumnTemplate : ITemplate
        {
            public string ImageUrl { get; set; }
            public string EditButtonID { get; set; }

            public void InstantiateIn(Control container)
            {
                HyperLink link = new HyperLink();
                container.Controls.Add(link);
                link.ID = EditButtonID;
                link.ImageUrl = ImageUrl;
            }
        }

        #endregion
    }
}
