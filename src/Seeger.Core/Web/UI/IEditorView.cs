using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public interface IEditorView<T>
    {
        void InitView(T entity);

        void UpdateObject(T entity);
    }
}
