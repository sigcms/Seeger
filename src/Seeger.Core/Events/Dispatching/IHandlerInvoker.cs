using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Events.Dispatching
{
    public interface IHandlerInvoker
    {
        void Invoke(IEvent evnt, MethodInfo handlerMethod, EventDispatchingContext context);
    }
}
