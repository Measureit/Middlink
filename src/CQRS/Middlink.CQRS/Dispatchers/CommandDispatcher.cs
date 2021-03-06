﻿using Microsoft.Extensions.DependencyInjection;
using Middlink.Core;
using Middlink.Core.CQRS.Commands;
using Middlink.Core.CQRS.Dispatchers;
using Middlink.Core.CQRS.Handlers;
using System.Threading.Tasks;

namespace Middlink.CQRS.Dispatchers
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
