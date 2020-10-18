using Middlink.Commands;
using Middlink.Events;
using Middlink.MessageBus;
using System.Threading.Tasks;

namespace Middlink.CQRS.MessageBus
{
    public class Publisher : IPublisher
    {
        private readonly IBusClient _busClient;

        public Publisher(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public Task SendAsync<TCommand>(TCommand command, ICorrelationContext context)
          where TCommand : ICommand
          => _busClient.PublishAsync(command, context);

        public Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context)
          where TEvent : IDomainEvent
          => _busClient.PublishAsync(@event, context);
    }
}
