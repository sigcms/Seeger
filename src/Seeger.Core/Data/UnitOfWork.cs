using NHibernate;
using NHibernate.Linq;
using Seeger.Events;
using Seeger.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISession Session { get; private set; }

        protected IEventDispatcher EventDispatcher { get; private set; }

        protected IList<IEvent> UncommittedEvents { get; private set; }

        public UnitOfWork(ISession session)
            : this(session, EventEnvironment.EventDispatcher)
        {
        }

        public UnitOfWork(ISession session, IEventDispatcher eventDispatcher)
        {
            Require.NotNull(session, "session");
            Require.NotNull(eventDispatcher, "eventDispatcher");

            Session = session;
            EventDispatcher = eventDispatcher;
            UncommittedEvents = new List<IEvent>();
        }

        public T Get<T>(object id)
        {
            return Session.Get<T>(id);
        }

        public IQueryable<T> Query<T>()
        {
            return Session.Query<T>();
        }

        public void Save(object entity)
        {
            Session.Save(entity);
        }

        public void Delete(object entity)
        {
            Session.Delete(entity);
        }

        public void EnqueueUncommittedEvent(IEvent evnt)
        {
            Require.NotNull(evnt, "evnt");

            UncommittedEvents.Add(evnt);
        }

        public void Commit()
        {
            DoCommit();

            // Post commit event handlers might invoke Commit() on current unit of work again.
            // In this case, we must ensure that all the event handlers should not be invoked again,
            // because current unit of work is invoking them.
            // So we copy uncommitted events to a temporary list here, then clear it before invoking handlers.
            var events = UncommittedEvents.ToList();
            UncommittedEvents.Clear();

            if (events.Count > 0)
            {
                // If some handler invokes domain model during the execution of handlers, new events might araise.
                // In this case, if the handler doesn't commit the unit of work, we simply ignore the new events.
                // This events will be removed when the current unit of work is disposed or the current thread is exited.
                // If the handler commit the unit of work, because we already clear the events above,
                // so only handlers for the new events will be invoked.
                DispatchPostCommitEvents(events);
            }
        }

        private void DispatchPostCommitEvents(IEnumerable<IEvent> events)
        {
            var context = new EventDispatchingContext(this, UnitOfWorkStage.Committed);

            foreach (var evnt in events)
            {
                EventDispatcher.Dispatch(evnt, context);
            }
        }

        private void DoCommit()
        {
            Session.Commit();
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
