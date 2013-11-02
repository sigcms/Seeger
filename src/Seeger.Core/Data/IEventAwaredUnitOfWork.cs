using Seeger.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data
{
    public interface IEventAwaredUnitOfWork : IUnitOfWork
    {
        void EnqueueUncommittedEvent(IEvent evnt);
    }
}
