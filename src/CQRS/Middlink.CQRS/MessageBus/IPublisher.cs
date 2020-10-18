using Middlink.Commands;
using Middlink.Events;
using System.Threading.Tasks;

namespace Middlink.CQRS.MessageBus
{
    public interface IPublisher
    {
        Task SendAsync<TCommand>(TCommand command, ICorrelationContext context)
            where TCommand : ICommand;

        Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context)
            where TEvent : IDomainEvent;

    }
}
