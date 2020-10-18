using Middlink.Collections;
using Middlink.CQRS.MessageBus;
using Middlink.Events;
using Middlink.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middlink.EventSourcing
{
    public class Session : ISession
    {
        private readonly AggregateTracker _aggregateTracker = new AggregateTracker();
        private readonly List<AggregateRoot> _aggregates = new List<AggregateRoot>();
        private readonly IEventStore _eventStore;
        private readonly IPublisher _publisher;

        public IReadOnlyList<AggregateRoot> Aggregates => _aggregates.AsReadOnly();

        public Session(
            IEventStore eventStore,
            IPublisher publisher)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public async Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : AggregateRoot, new()
        {
            var aggregate = _aggregateTracker.GetById<TAggregate>(id);

            if (aggregate != null)
            {
                RegisterForTracking(aggregate);

                return aggregate;
            }

            aggregate = new TAggregate();

            IEnumerable<ICommittedEvent> events = await _eventStore.GetAllEventsAsync(id).ConfigureAwait(false);
            LoadAggregate(aggregate, events);

            if (aggregate.Id.Equals(Guid.Empty))
            {
                throw new AggregateNotFoundException(typeof(TAggregate).Name, id);
            }
            RegisterForTracking(aggregate);
            return aggregate;
        }

        public Task AddAsync<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
        {
            RegisterForTracking(aggregate);
            return Task.CompletedTask;
        }

        public virtual Task SaveChangesAsync(ICorrelationContext context) => Task.WhenAll(_aggregates.Select(aggregate => this.SaveAggregateChangesAsync(context, aggregate)));


        private async Task SaveAggregateChangesAsync(ICorrelationContext context, AggregateRoot aggregate)
        {
            var uncommittedEvents = aggregate.UncommittedEvents
                .OrderBy(o => o.CreatedAt)
                .Cast<UncommittedEvent>()
                .Select(GenerateMetadata)
                .ToList();

            await _eventStore.AppendAsync(aggregate, uncommittedEvents);

            foreach (var @event in uncommittedEvents)
            {
                await _publisher.PublishAsync(@event.Data, context);
            }
            await _eventStore.Commit(aggregate);
            //ToDo remove events from store if failed
            aggregate.UpdateVersion(aggregate.Sequence);
            aggregate.ClearUncommittedEvents();

        }

        private UncommittedEvent GenerateMetadata(UncommittedEvent uncommittedEvent)
        {
            uncommittedEvent.Metadata = new MetadataCollection(new[]
                        {
                            new KeyValuePair<string, object>(MetadataKeys.EventId, Guid.NewGuid()),
                            new KeyValuePair<string, object>(MetadataKeys.EventVersion, uncommittedEvent.Version),
                            new KeyValuePair<string, object>(MetadataKeys.Timestamp, DateTime.UtcNow),
                            new KeyValuePair<string, object>(MetadataKeys.EventClrType, uncommittedEvent.Data.GetType().AssemblyQualifiedName),
              new KeyValuePair<string, object>(MetadataKeys.EventName, uncommittedEvent.Data.GetType().Name)
        });


            return uncommittedEvent;
        }

        private void RegisterForTracking<TAggregate>(TAggregate aggregateRoot) where TAggregate : AggregateRoot
        {
            if (_aggregates.All(e => e.Id != aggregateRoot.Id))
            {
                _aggregates.Add(aggregateRoot);
            }
            _aggregateTracker.Add(aggregateRoot);
        }

        private void LoadAggregate<TAggregate>(TAggregate aggregate, IEnumerable<ICommittedEvent> committedEvents) where TAggregate : AggregateRoot
        {
            var flatten = committedEvents as ICommittedEvent[] ?? committedEvents.ToArray();

            if (flatten.Any())
            {
                aggregate.LoadFromHistory(new CommittedEventsCollection(flatten.Select(e => e.Data)));

                aggregate.UpdateVersion(flatten.Select(e => e.Version).Max());
            }
        }

        private void Reset()
        {
            _aggregates.Clear();
        }
    }
}
