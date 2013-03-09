using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

using Seeger.Data;
using Seeger.Web.UI;
using NHibernate;

namespace Seeger.Web.UI.DataManagement
{
    public abstract class DetailPageBase<TEntity> : AdminPageBase, IFormView<TEntity>
        where TEntity : class
    {
        private static readonly string _idQueryStringParam = "id";

        protected abstract object CreateKey(string keyStringValue);

        private TEntity _entity;
        protected TEntity Entity
        {
            get
            {
                if (_entity == null)
                {
                    string keyStr = Request.QueryString[_idQueryStringParam];
                    if (!String.IsNullOrEmpty(keyStr))
                    {
                        object key = CreateKey(keyStr);
                        _entity = NhSession.Get<TEntity>(key);
                        if (_entity == null)
                        {
                            throw new InvalidOperationException(String.Format("Entity with key '{0}' was not found.", keyStr));
                        }
                    }
                    else
                    {
                        _entity = CreateEntity();
                    }
                }
                return _entity;
            }
        }

        protected virtual TEntity CreateEntity()
        {
            return (TEntity)Activator.CreateInstance(typeof(TEntity));
        }

        public FormState FormState
        {
            get
            {
                return !String.IsNullOrEmpty(Request.QueryString[_idQueryStringParam]) ? FormState.EditItem : FormState.AddItem;
            }
        }


        public abstract void InitView(TEntity entity);

        public abstract void UpdateObject(TEntity entity);

        protected abstract void BindSubmitEventHandler(EventHandler handler);

        public void Submit()
        {
            if (!IsValid)
            {
                return;
            }

            UpdateObject(Entity);

            if (FormState == FormState.AddItem)
            {
                NhSession.Save(Entity);
            }

            NhSession.Commit();

            OnSubmitted();
        }

        protected virtual void OnSubmitted()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            BindSubmitEventHandler(new EventHandler((obj, args) => { Submit(); }));

            if (!IsPostBack)
            {
                InitView(Entity);
            }
        }
    }
}
