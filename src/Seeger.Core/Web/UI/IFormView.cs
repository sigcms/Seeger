using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public enum FormState
    {
        AddItem,
        EditItem
    }

    public interface IFormView<TEntity> : IEditorView<TEntity>
        where TEntity : class
    {
        FormState FormState { get; }

        void Submit();
    }
}
