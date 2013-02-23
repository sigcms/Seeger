using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Context
{
    public interface INhSessionContext
    {
        ISession GetCurrentSession();

        void CloseCurrentSession();
    }

    public static class NhSessionContexts
    {
        public static INhSessionContext Current = new WebNhSessionContext();
    }
}
