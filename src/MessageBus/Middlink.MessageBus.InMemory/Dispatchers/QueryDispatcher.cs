using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Middlink.MessageBus.Dispatchers;
using Middlink.MessageBus.Handlers;
using Middlink.Messages.Queries;


namespace Middlink.MessageBus.InMemory.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public QueryDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

                dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

                return await handler.HandleAsync((dynamic)query);
            }
        }
    }
}
