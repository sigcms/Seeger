using Seeger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Events
{
    public interface IUnitOfWorkAware
    {
        IUnitOfWork UnitOfWork { get; set; }
    }
}
