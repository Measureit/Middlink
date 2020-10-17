using Microsoft.Extensions.DependencyInjection;
using Middlink.Dispatchers;
using Middlink.Handlers;
using Middlink.Messages;
using Middlink.Messages.Commands;
using System.Threading.Tasks;

namespace Middlink.Impl.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public CommandDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }


        public async Task DispatchAsync<TCommand>(TCommand command, ICorrelationContext context) where TCommand : ICommand
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
                await handler.HandleAsync(command, context);
            }
        }
    }
}
