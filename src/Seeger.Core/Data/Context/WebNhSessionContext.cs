using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Web;

namespace Seeger.Data.Context
{
    public class WebNhSessionContext : INhSessionContext
    {
        const string Key = "Seeger.Data.Context.WebNhSessionContext";

        public ISession GetCurrentSession()
        {
            var context = GetHttpContextWithAssert();

            var session = context.Items[Key] as ISession;
            if (session == null)
            {
                session = Database.OpenSession();
                context.Items[Key] = session;
            }

            return session;
        }

        public void CloseCurrentSession()
        {
            var context = GetHttpContextWithAssert();
            var session = context.Items[Key] as ISession;
            if (session != null)
            {
                session.Close();
            }
        }

        private static HttpContext GetHttpContextWithAssert()
        {
            var context = HttpContext.Current;

            if (context == null)
                throw new InvalidOperationException("Cannot obtain current session because because HttpContext is currently not available.");

            return context;
        }
    }
}
