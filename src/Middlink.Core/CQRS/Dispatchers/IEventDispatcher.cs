using Middlink.Core.CQRS.Events;
using System.Threading.Tasks;

namespace Middlink.Core.CQRS.Dispatchers
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent @event, ICorrelationContext context) where TEvent : IDomainEvent;
    }
}
