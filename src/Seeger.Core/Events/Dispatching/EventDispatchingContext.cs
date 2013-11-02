using Seeger.Data;
using System;

namespace Seeger.Events.Dispatching
{
    public class EventDispatchingContext
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public UnitOfWorkStage UnitOfWorkStage { get; private set; }

        public EventDispatchingContext(IUnitOfWork unitOfWork, UnitOfWorkStage stage)
        {
            // Unit of work can be null
            UnitOfWork = unitOfWork;
            UnitOfWorkStage = stage;
        }
    }

    public enum UnitOfWorkStage
    {
        Uncommitted = 0,
        Committed = 1
    }
}
