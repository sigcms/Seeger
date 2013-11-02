using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Events
{
    public interface IHandle<TEvent>
        where TEvent : IEvent
    {
        void Handle(TEvent evnt);
    }
}
