using Middlink.Messages;
using Middlink.Messages.Commands;
using Middlink.Messages.Events;
using System.Threading.Tasks;

namespace Middlink.MessageBus.Services
{
    public interface IBusPublisher
    {
        Task SendAsync<TCommand>(TCommand command, ICorrelationContext context)
            where TCommand : ICommand;

        Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context)
            where TEvent : IDomainEvent;
    }
}
