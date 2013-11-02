using System;

namespace Seeger.Events.Dispatching
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent evnt, EventDispatchingContext context);
    }
}
