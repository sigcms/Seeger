using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI.DataManagement
{
    public interface IGridViewCallback<TEntity>
    {
        void OnItemDeleted(TEntity entity);
    }

    internal class NullGridViewCallback<TEntity> : IGridViewCallback<TEntity>
    {
        public static readonly NullGridViewCallback<TEntity> Instance = new NullGridViewCallback<TEntity>();

        public void OnItemDeleted(TEntity entity)
        {
        }
    }
}
