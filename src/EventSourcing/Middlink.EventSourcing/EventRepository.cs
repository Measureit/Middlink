using System;
using System.Threading.Tasks;

namespace Middlink.EventSourcing
{
    public class EventRepository
    {
        private readonly ISession _session;

        public EventRepository(ISession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            _session = session;
        }

        public Task AddAsync<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
        => _session.AddAsync(aggregate);

        public Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : AggregateRoot, new()
        => _session.GetByIdAsync<TAggregate>(id);
    }
}
