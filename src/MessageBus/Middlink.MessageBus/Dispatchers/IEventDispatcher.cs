using System.Threading.Tasks;
using Middlink.Messages;
using Middlink.Messages.Events;

namespace Middlink.MessageBus.Dispatchers
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent @event, ICorrelationContext context) where TEvent : IDomainEvent;
    }
}
