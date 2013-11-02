using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Events.Dispatching
{
    public interface IHandlerRegistry
    {
        IEnumerable<MethodInfo> FindHandlerMethods(string group, Type eventType);

        void RegisterHandlers(string group, IEnumerable<Type> handlerTypes);

        void RegisterHandlers(string group, IEnumerable<Assembly> assemblies);

        bool RemoveHandlers(string group);

        bool RemoveHandlers(string group, Type eventType);
    }
}
