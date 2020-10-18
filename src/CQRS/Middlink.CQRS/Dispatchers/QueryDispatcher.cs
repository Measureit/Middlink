using Microsoft.Extensions.DependencyInjection;
using Middlink.CQRS.Handlers;
using Middlink.Queries;
using System.Threading.Tasks;


namespace Middlink.CQRS.Dispatchers
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
