using Seeger.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Events
{
    public static class EventEnvironment
    {
        public static IEventDispatcher EventDispatcher { get; set; }

        public static IHandlerRegistry HandlerRegistry { get; set; }

        static EventEnvironment()
        {
            HandlerRegistry = new DefaultHandlerRegistry();
            EventDispatcher = new DefaultEventDispatcher(HandlerRegistry);

            HandlerRegistry.RegisterHandlers(EventConstants.SystemHandlerGroup, new[] { typeof(EventEnvironment).Assembly });
        }
    }
}
