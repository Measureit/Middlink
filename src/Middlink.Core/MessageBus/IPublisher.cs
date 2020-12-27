using Middlink.Core.CQRS.Commands;
using Middlink.Core.CQRS.Events;
using System.Threading.Tasks;

namespace Middlink.Core.MessageBus
{
    public interface IPublisher
    {
        Task SendAsync<TCommand>(TCommand command, ICorrelationContext context)
            where TCommand : ICommand;

        Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context)
            where TEvent : IDomainEvent;

    }
}
