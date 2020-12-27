using Middlink.Core.CQRS.Events;
using System.Threading.Tasks;

namespace Middlink.Core.CQRS.Handlers
{
    public interface IEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext context);
    }
}
