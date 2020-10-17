using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Middlink.MessageBus.Dispatchers;
using Middlink.MessageBus.Handlers;
using Middlink.Messages;
using Middlink.Messages.Events;

namespace Middlink.MessageBus.InMemory.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public EventDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }


        public async Task DispatchAsync<TEvent>(TEvent @event, ICorrelationContext context) where TEvent : IDomainEvent
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
                foreach (var handler in handlers)
                {
                    await handler.HandleAsync(@event, context);
                }
            }
        }
    }
}
