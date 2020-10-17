using Microsoft.AspNetCore.Builder;
using Middlink.MessageBus.MessageBrokers;
using Middlink.MessageBus.Services;
using System;

namespace Middlink.MessageBus.InMemory.Extensions
{
    public static class Extensions
    {
        public static IBusSubscriber UseInMemoryQueue(this IApplicationBuilder app) => new BusSubscriber(app.ApplicationServices);
    }
}
