using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

using Seeger.Data;

namespace Seeger.Web.UI.DataManagement
{
    public abstract class ListUserControlBase<TEntity> : UserControl
        where TEntity : class
    {
        protected abstract GridView GridView { get; }

        protected virtual int PageIndex
        {
            get { return Request.QueryString.TryGetValue<int>("page", 0); }
        }

        protected virtual string EntityKeyPropertyName
        {
            get { return EntityKeyCache.GetEntityKeyPropertyName(typeof(TEntity)); }
        }

        protected virtual bool AllowPaging
        {
            get { return true; }
        }

        protected virtual bool AllowDeletion
        {
            get { return true; }
        }

        protected virtual bool AllowEditing
        {
            get { return true; }
        }

        protected virtual string EditingPageUrl
        {
            get { return typeof(TEntity).Name + "Edit.aspx"; }
        }

        protected virtual string PagerUrlFormat
        {
            get { return Request.Path + "?page={0}"; }
        }

        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> src)
        {
            return src;
        }

        private GridViewWrapper<TEntity> _wrapper;

        protected GridViewWrapper<TEntity> GridViewWrapper
        {
            get
            {
                if (_wrapper == null)
                {
                    _wrapper = new GridViewWrapper<TEntity>(GridView, NhSessionManager.GetCurrentSession());
                    _wrapper.PageIndex = PageIndex;
                    _wrapper.PagerUrlFormat = PagerUrlFormat;
                    _wrapper.AllowPaging = AllowPaging;
                    _wrapper.AllowDeletion = AllowDeletion;
                    _wrapper.AllowEditing = AllowEditing;
                    _wrapper.EditingPageUrl = EditingPageUrl;
                    _wrapper.EntityKeyPropertyName = EntityKeyPropertyName;
                    _wrapper.PageIndex = PageIndex;
                    _wrapper.DataSourceFilter = new Func<IQueryable<TEntity>, IQueryable<TEntity>>(Filter);

                    if (this is IRecordFilter<TEntity>)
                    {
                        _wrapper.RecordFilter = (IRecordFilter<TEntity>)this;
                    }

                    if (this is IGridViewCallback<TEntity>)
                    {
                        _wrapper.Callback = (IGridViewCallback<TEntity>)this;
                    }
                }

                return _wrapper;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            GridViewWrapper.DataBind();
        }
    }
}
