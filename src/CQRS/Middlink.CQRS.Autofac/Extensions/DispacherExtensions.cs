﻿using Autofac;
using Middlink.Core.CQRS.Dispatchers;
using Middlink.CQRS.Dispatchers;

namespace Middlink.CQRS.Autofac.Extensions
{
    public static class DispacherExtensions
    {
        public static void AddDispachers(this ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>().InstancePerLifetimeScope();
            builder.RegisterType<EventDispatcher>().As<IEventDispatcher>().InstancePerLifetimeScope();
            builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>().InstancePerLifetimeScope();
        }
    }
}
