using Middlink.Collections;
using System;

namespace Middlink.Events
{
    public class UncommittedEvent : IUncommittedEvent
    {
        private readonly long _ticks;
        public DateTime CreatedAt => new DateTime(_ticks);
        public Guid AggregateId { get; }
        public IDomainEvent Data { get; }
        public int Version { get; }
        public IMetadataCollection Metadata { get; set; } = MetadataCollection.Empty;

        public UncommittedEvent(Guid aggregateId, IDomainEvent @event, int version) :
            this(aggregateId, @event, version, DateTime.Now.Ticks, MetadataCollection.Empty)
        {
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private UncommittedEvent(Guid aggregateId, IDomainEvent @event, int version, long ticks, IMetadataCollection metadata)
        {
            AggregateId = aggregateId;
            Data = @event;
            Version = version;
            Metadata = Metadata.Merge(metadata);
            _ticks = ticks;
        }
    }
}
