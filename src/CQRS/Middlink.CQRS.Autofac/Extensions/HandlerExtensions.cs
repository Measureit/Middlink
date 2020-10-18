using Autofac;
using Microsoft.AspNetCore.Builder;
using Middlink.CQRS.Handlers;
using Middlink.CQRS.MessageBus;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Middlink.CQRS.Autofac.Extensions
{
    public static class HandlerExtensions
    {
        public static ISubscriber UseCQRS(this IApplicationBuilder serviceProvider)
        {
            return new Subscriber(serviceProvider.ApplicationServices);
        }
        public static void AddCQRS(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            foreach(var assembly in assemblies)
            {
                builder.AddHandlers(assembly);
            }
            builder.AddDispachers();
            builder.AddPublisher();
        }
        public static void AddPublisher(this ContainerBuilder builder)
        {
            builder.RegisterType<Publisher>().As<IPublisher>().InstancePerLifetimeScope();
        }

        public static void AddHandlers(this ContainerBuilder builder, Assembly assembly)
        {
            builder.AddEventHandlers(assembly);
            builder.AddCommandHandlers(assembly);
            builder.AddQueryHandlers(assembly);
        }

        public static void AddEventHandlers(this ContainerBuilder builder, Assembly assembly)
        {
            //var assembly = Assembly.GetCallingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
        public static void AddCommandHandlers(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
        public static void AddQueryHandlers(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}
