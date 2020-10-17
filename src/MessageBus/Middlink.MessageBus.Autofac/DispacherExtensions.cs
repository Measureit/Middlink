using Autofac;
using Middlink.MessageBus.Dispatchers;

namespace Middlink.MessageBus.Autofac
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
