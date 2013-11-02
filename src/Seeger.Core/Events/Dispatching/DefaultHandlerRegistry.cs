using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Seeger.Events.Dispatching
{
    public class DefaultHandlerRegistry : IHandlerRegistry
    {
        private readonly object _writeLock = new object();
        private Dictionary<string, Dictionary<Type, List<MethodInfo>>> _handlersByGroup = new Dictionary<string, Dictionary<Type, List<MethodInfo>>>();

        public IEnumerable<MethodInfo> FindHandlerMethods(string group, Type eventType)
        {
            Require.NotNull(eventType, "eventType");
            Require.NotNullOrEmpty(group, "group");

            // No lock is needed, cos we use copy-on-write to perform handler registration
            var handlersByGroup = _handlersByGroup;

            var result = new List<MethodInfo>();

            result.AddRange(FindDirectHandlers(eventType, group, handlersByGroup));

            // Here we need to support base event subscribtion:
            // If event A is raised, handlers subscribing to A and A's base events all need to be invoked.
            var baseEventType = eventType.BaseType;

            while (baseEventType != null && typeof(IEvent).IsAssignableFrom(baseEventType))
            {
                result.AddRange(FindDirectHandlers(baseEventType, group, handlersByGroup));
                baseEventType = baseEventType.BaseType;
            }

            return result;
        }

        private IEnumerable<MethodInfo> FindDirectHandlers(Type eventType, string group, Dictionary<string, Dictionary<Type, List<MethodInfo>>> handlersByGroup)
        {
            Dictionary<Type, List<MethodInfo>> handlersByEventType;

            if (!handlersByGroup.TryGetValue(group, out handlersByEventType))
            {
                return Enumerable.Empty<MethodInfo>();
            }

            List<MethodInfo> handlerMethods;

            if (!handlersByEventType.TryGetValue(eventType, out handlerMethods))
            {
                return Enumerable.Empty<MethodInfo>();
            }

            return handlerMethods;
        }

        public void RegisterHandlers(string group, IEnumerable<Type> handlerTypes)
        {
            lock (_writeLock)
            {
                var newHandlersByEventType = new Dictionary<Type, List<MethodInfo>>();

                foreach (var type in handlerTypes)
                {
                    RegisterHandler(type, newHandlersByEventType);
                }

                if (newHandlersByEventType.Count > 0)
                {
                    // Copy on Write
                    var handlersByGroup = Clone();

                    Dictionary<Type, List<MethodInfo>> currentHandlersByEventType;

                    if (!handlersByGroup.TryGetValue(group, out currentHandlersByEventType))
                    {
                        currentHandlersByEventType = new Dictionary<Type, List<MethodInfo>>();
                        handlersByGroup.Add(group, currentHandlersByEventType);
                    }

                    foreach (var kv in newHandlersByEventType)
                    {
                        List<MethodInfo> currentHandlerMethods;

                        if (!currentHandlersByEventType.TryGetValue(kv.Key, out currentHandlerMethods))
                        {
                            currentHandlerMethods = new List<MethodInfo>();
                            currentHandlersByEventType.Add(kv.Key, currentHandlerMethods);
                        }

                        currentHandlerMethods.AddRange(kv.Value);
                    }

                    _handlersByGroup = handlersByGroup;
                }
            }
        }

        public void RegisterHandlers(string group, IEnumerable<Assembly> assemblies)
        {
            Require.NotNull(assemblies, "assemblies");

            var types = new List<Type>();

            foreach (var asm in assemblies)
            {
                foreach (var type in asm.GetTypes())
                {
                    // Do a simple filter to reduce the size of 'types' collection
                    if (type.IsClass && !type.IsAbstract)
                    {
                        types.Add(type);
                    }
                }
            }

            if (types.Count > 0)
            {
                RegisterHandlers(group, types);
            }
        }

        private void RegisterHandler(Type handlerType, Dictionary<Type, List<MethodInfo>> handlersByEventType)
        {
            Require.NotNull(handlerType, "handlerType");

            if (!handlerType.IsClass || handlerType.IsAbstract) return;

            var eventTypes = TypeUtil.GetOpenGenericArgumentTypes(handlerType, typeof(IHandle<>)).ToList();

            if (eventTypes.Count == 0) return;

            var thisHandlerMethods = handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                .Where(m => m.Name == "Handle" && m.ReturnType == typeof(void))
                                                .ToList();

            foreach (var eventType in eventTypes)
            {
                List<MethodInfo> handlerMethods = null;

                if (!handlersByEventType.TryGetValue(eventType, out handlerMethods))
                {
                    handlerMethods = new List<MethodInfo>();
                    handlersByEventType.Add(eventType, handlerMethods);
                }

                foreach (var method in thisHandlerMethods)
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length == 1 && parameters[0].ParameterType == eventType)
                    {
                        handlerMethods.Add(method);
                        break;
                    }
                }
            }
        }

        public bool RemoveHandlers(string group)
        {
            lock (_writeLock)
            {
                if (_handlersByGroup.ContainsKey(group))
                {
                    // Copy on Write
                    var handlersByGroup = Clone();
                    handlersByGroup.Remove(group);

                    _handlersByGroup = handlersByGroup;

                    return true;
                }
            }

            return false;
        }

        public bool RemoveHandlers(string group, Type eventType)
        {
            lock (_writeLock)
            {
                if (_handlersByGroup.ContainsKey(group))
                {
                    var handlersByEventType = _handlersByGroup[group];

                    if (handlersByEventType.ContainsKey(eventType))
                    {
                        // Copy on Write
                        var handlersByGroup = Clone();
                        handlersByEventType = handlersByGroup[group];
                        handlersByEventType.Remove(eventType);

                        _handlersByGroup = handlersByGroup;

                        return true;
                    }
                }
            }

            return false;
        }

        private Dictionary<string, Dictionary<Type, List<MethodInfo>>> Clone()
        {
            var handlersByGroup = new Dictionary<string, Dictionary<Type, List<MethodInfo>>>();

            foreach (var each in _handlersByGroup)
            {
                handlersByGroup.Add(each.Key, Clone(each.Value));
            }

            return handlersByGroup;
        }

        private Dictionary<Type, List<MethodInfo>> Clone(Dictionary<Type, List<MethodInfo>> data)
        {
            var result = new Dictionary<Type, List<MethodInfo>>();

            foreach (var kv in data)
            {
                result.Add(kv.Key, new List<MethodInfo>(kv.Value));
            }

            return result;
        }
    }
}
