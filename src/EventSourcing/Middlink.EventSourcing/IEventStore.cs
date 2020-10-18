using Middlink.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middlink.EventSourcing
{
    public interface IEventStore : IDisposable
    {
        Task<IEnumerable<ICommittedEvent>> GetAllEventsAsync(Guid id);

        Task<IEnumerable<ICommittedEvent>> GetEventsForwardAsync(Guid id, int version);

        Task AppendAsync(AggregateRoot aggregate, IEnumerable<IUncommittedEvent> uncommittedEvents);
        Task Commit(AggregateRoot aggregate);
    }
}
