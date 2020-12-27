using Microsoft.Extensions.DependencyInjection;
using Middlink.Core;
using Middlink.Core.CQRS.Dispatchers;
using Middlink.Core.CQRS.Events;
using Middlink.Core.CQRS.Handlers;
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
