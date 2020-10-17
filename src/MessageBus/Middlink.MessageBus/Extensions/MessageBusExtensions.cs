using Middlink.MessageBus.Services;
using Middlink.Messages;
using Middlink.Messages.Commands;
using Middlink.Messages.Events;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Middlink.MessageBus.Extensions
{
    public static class MessageBusExtensions
    {
        public static IBusSubscriber SubscribeAllMessages(this IBusSubscriber subscriber, IEnumerable<Assembly> messagesAssembly)
            => subscriber.SubscribeAllCommands(messagesAssembly).SubscribeAllEvents(messagesAssembly);

        public static IBusSubscriber SubscribeAllCommands(this IBusSubscriber subscriber, IEnumerable<Assembly> messagesAssembly)
            => subscriber.SubscribeAllMessages<ICommand>(messagesAssembly, nameof(IBusSubscriber.SubscribeCommand));

        public static IBusSubscriber SubscribeAllEvents(this IBusSubscriber subscriber, IEnumerable<Assembly> messagesAssembly)
            => subscriber.SubscribeAllMessages<IDomainEvent>(messagesAssembly, nameof(IBusSubscriber.SubscribeEvent));

        private static IBusSubscriber SubscribeAllMessages<TMessage>
            (this IBusSubscriber subscriber, IEnumerable<Assembly> messagesAssembly, string subscribeMethod)
        {
            var messageTypes = messagesAssembly.SelectMany(assembly =>
                assembly.GetTypes()
                .Where(t => t.IsClass && typeof(TMessage).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract))
                .ToList();

            messageTypes.ForEach(mt => subscriber.GetType()
                .GetMethod(subscribeMethod)
                .MakeGenericMethod(mt)
                .Invoke(subscriber,
                    new object[] { mt.GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace, null, null }));

            return subscriber;
        }
    }
}

