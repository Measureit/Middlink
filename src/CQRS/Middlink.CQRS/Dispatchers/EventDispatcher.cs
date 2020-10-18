using Microsoft.Extensions.DependencyInjection;
using Middlink.CQRS.Handlers;
using Middlink.Events;
using System.Threading.Tasks;

namespace Middlink.CQRS.Dispatchers
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
