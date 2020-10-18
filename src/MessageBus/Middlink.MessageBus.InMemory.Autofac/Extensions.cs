﻿using Autofac;

namespace Middlink.MessageBus.InMemory.Autofac
{
    public static class Extensions
    {
        public static void AddInMemoryMessageBus(this ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryBusClient>().As<IBusClient>().SingleInstance();
        }
    }
}
