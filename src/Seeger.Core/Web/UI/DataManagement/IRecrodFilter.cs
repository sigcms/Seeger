using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.DataManagement
{
    public interface IRecordFilter<T>
    {
        bool IsVisible(T record);

        bool IsEditable(T record);

        bool IsDeletable(T record);
    }

    internal class NullRecordFilter<T> : IRecordFilter<T>
    {
        public static readonly NullRecordFilter<T> Instance = new NullRecordFilter<T>();

        public bool IsVisible(T record)
        {
            return true;
        }

        public bool IsEditable(T record)
        {
            return true;
        }

        public bool IsDeletable(T record)
        {
            return true;
        }
    }
}
