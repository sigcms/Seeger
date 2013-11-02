using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data
{
    public class UnitOfWorkScope : IDisposable
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public UnitOfWorkScope()
            : this(new UnitOfWork(Database.OpenSession()))
        {
        }

        public UnitOfWorkScope(IUnitOfWork unitOfWork)
        {
            Require.NotNull(unitOfWork, "unitOfWork");
            Bind(unitOfWork);
        }

        private void Bind(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            UnitOfWorkAmbient.Bind(unitOfWork);
        }

        public void Commit()
        {
            UnitOfWork.Commit();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
            UnitOfWorkAmbient.Unbind();
        }
    }
}
