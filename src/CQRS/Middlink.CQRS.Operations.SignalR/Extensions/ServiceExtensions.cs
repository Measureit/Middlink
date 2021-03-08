using Microsoft.Extensions.DependencyInjection;
using Middlink.Core.CQRS.Handlers;
using Middlink.Core.CQRS.Operations;
using Middlink.CQRS.Operations.SignalR.Services.Operations;
using System;

namespace Middlink.CQRS.Operations.SignalR.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddOperations(this IServiceCollection builder)
        {
            builder.AddTransient<IOperationPublisher, OperationPublisher>();
            builder.AddSingleton<IOperationsStorage, OperationsStorage>();
            builder.AddTransient(typeof(IEventHandler<>), typeof(OperationEventHandler<>));
        }
    }
}
