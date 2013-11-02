using Seeger.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data
{
    public interface IUnitOfWork : IDisposable
    {
        T Get<T>(object id);

        IQueryable<T> Query<T>();

        void Save(object entity);

        void Delete(object entity);

        void Commit();
    }
}
