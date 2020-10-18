using Middlink.Events;
using System.Threading.Tasks;

namespace Middlink.CQRS.Handlers
{
    public interface IEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext context);
    }
}
