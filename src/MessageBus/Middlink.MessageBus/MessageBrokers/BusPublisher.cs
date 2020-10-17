using Middlink.MessageBus.Services;
using Middlink.Messages;
using Middlink.Messages.Commands;
using Middlink.Messages.Events;
using System.Threading.Tasks;

namespace Middlink.MessageBus.MessageBrokers
{
    public class BusPublisher : IBusPublisher
    {
        private readonly IBusClient _busClient;

        public BusPublisher(IBusClient busClient)
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
