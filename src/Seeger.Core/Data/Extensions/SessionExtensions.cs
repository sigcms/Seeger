using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public static class SessionExtensions
    {
        public static void Commit(this ISession session)
        {
            using (var tran = session.BeginTransaction())
            {
                tran.Commit();
            }
        }
    }
}
