using Middlink.Events;
using System.Threading.Tasks;

namespace Middlink.CQRS.Dispatchers
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent @event, ICorrelationContext context) where TEvent : IDomainEvent;
    }
}
