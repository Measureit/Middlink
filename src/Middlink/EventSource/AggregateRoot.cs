using Middlink.Messages.Collections;
using Middlink.Messages.Events;
using System;
using System.Collections.Generic;

namespace Middlink.EventSource
{
    public abstract class AggregateRoot
    {
        private readonly List<UncommittedEvent> _uncommittedEvents = new List<UncommittedEvent>();
        private readonly Route<IDomainEvent> _routeEvents;
        protected virtual bool HandleIsMandatory { get; } = true;
        public int Sequence => Version + _uncommittedEvents.Count;
        public IReadOnlyCollection<IUncommittedEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();
        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        protected AggregateRoot()
        {
            _routeEvents = new Route<IDomainEvent>(HandleIsMandatory);

            RegisterEvents();
        }
        protected abstract void RegisterEvents();
        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public void LoadFromHistory(CommittedEventsCollection domainEvents)
        {
            foreach (var e in domainEvents) ApplyEvent(e);
        }

        private void ApplyEvent(IDomainEvent @event)
        {
            _routeEvents.Handle(@event);
        }

        private void ApplyEvent(IDomainEvent @event, bool isNew = false)
        {
            this.ApplyEvent(@event);

            if (isNew)
            {
                _uncommittedEvents.Add(new UncommittedEvent(this.Id, @event, Sequence + 1));
            }
        }
        protected void SubscribeTo<T>(Action<T> action) where T : class, IDomainEvent
        {
            _routeEvents.Add(typeof(T), o => action(o as T));
        }

        protected void Emit(IDomainEvent @event)
        {
            ApplyEvent(@event, true);
        }

        internal void UpdateVersion(int version)
        {
            Version = version;
        }
    }
}