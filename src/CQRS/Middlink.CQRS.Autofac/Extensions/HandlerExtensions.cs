using Autofac;
using Middlink.CQRS.Handlers;
using System.Reflection;

namespace Middlink.CQRS.Autofac.Extensions
{
    public static class HandlerExtensions
    {
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
