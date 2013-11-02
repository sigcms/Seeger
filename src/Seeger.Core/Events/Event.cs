using Seeger.Data;
using Seeger.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Events
{
    [Serializable]
    public static class Event
    {
        public static void Raise<TEvent>(TEvent evnt)
            where TEvent : IEvent
        {
            Require.NotNull(evnt, "evnt");

            var dispatcher = EventEnvironment.EventDispatcher;

            if (dispatcher == null)
                throw new InvalidOperationException("Event dispatcher is not registered.");

            var unitOfWork = (IUnitOfWork)UnitOfWorkAmbient.Current;

            dispatcher.Dispatch(evnt, new EventDispatchingContext(unitOfWork, UnitOfWorkStage.Uncommitted));

            var eventAwareUnitOfWork = unitOfWork as IEventAwaredUnitOfWork;

            if (eventAwareUnitOfWork != null)
                eventAwareUnitOfWork.EnqueueUncommittedEvent(evnt);
        }
    }
}
