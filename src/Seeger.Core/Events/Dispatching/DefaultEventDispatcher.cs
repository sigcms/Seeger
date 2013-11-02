using Seeger.Plugins;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Seeger.Events.Dispatching
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private IHandlerRegistry _handlerRegistry;
        private IHandlerInvoker _handlerInvoker;

        public IHandlerRegistry HandlerRegistry
        {
            get
            {
                return _handlerRegistry;
            }
        }

        public IHandlerInvoker HandlerInvoker
        {
            get
            {
                return _handlerInvoker;
            }
        }

        public DefaultEventDispatcher()
            : this(new DefaultHandlerRegistry())
        {
        }

        public DefaultEventDispatcher(IHandlerRegistry handlerRegistry)
            : this(handlerRegistry, new DefaultHandlerInvoker())
        {
        }

        public DefaultEventDispatcher(IHandlerRegistry handlerRegistry, IHandlerInvoker handlerInvoker)
        {
            Require.NotNull(handlerRegistry, "handlerRegistry");
            Require.NotNull(handlerInvoker, "handlerInvoker");

            _handlerRegistry = handlerRegistry;
            _handlerInvoker = handlerInvoker;
        }

        public void Dispatch(IEvent evnt, EventDispatchingContext context)
        {
            Require.NotNull(evnt, "evnt");
            Require.NotNull(context, "context");

            var eventType = evnt.GetType();

            // Dispatch event to system handlers
            Dispatch(evnt, context, _handlerRegistry.FindHandlerMethods(EventConstants.SystemHandlerGroup, eventType));

            // Dispatch event to plugin handlers
            foreach (var plugin in PluginManager.EnabledPlugins)
            {
                Dispatch(evnt, context, _handlerRegistry.FindHandlerMethods(plugin.Name, eventType));
            }
        }

        private void Dispatch(IEvent evnt, EventDispatchingContext context, IEnumerable<MethodInfo> handlerMethods)
        {
            foreach (var method in handlerMethods)
            {
                var awaitCommit = TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(method, typeof(AwaitCommittedAttribute));

                if (awaitCommit && context.UnitOfWorkStage != UnitOfWorkStage.Committed)
                {
                    continue;
                }

                if (!awaitCommit && context.UnitOfWorkStage == UnitOfWorkStage.Committed)
                {
                    continue;
                }

                _handlerInvoker.Invoke(evnt, method, context);
            }
        }
    }
}
