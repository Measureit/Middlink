using Middlink.Core;
using System;
using System.Threading.Tasks;

namespace Middlink.EventSourcing
{
    public interface ISession
    {

        Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : AggregateRoot, new();

        Task AddAsync<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot;

        Task SaveChangesAsync(ICorrelationContext context);

    }
}
