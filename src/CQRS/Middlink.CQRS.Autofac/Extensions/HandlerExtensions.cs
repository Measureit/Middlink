using Autofac;
using Microsoft.AspNetCore.Builder;
using Middlink.Commands;
using Middlink.CQRS.Handlers;
using Middlink.CQRS.MessageBus;
using Middlink.Events;
using Middlink.Exceptions;
using Middlink.MessageBus;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Middlink.CQRS.Autofac.Extensions
{
  public interface ICQRSBuilder
  {
    ICQRSBuilder SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
                  Func<TCommand, MiddlinkException, IRejectedEvent> onError = null)
                  where TCommand : ICommand;

    ICQRSBuilder SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
                Func<TEvent, MiddlinkException, IRejectedEvent> onError = null)
                where TEvent : IDomainEvent;

    ICQRSBuilder SubscribeAllMessages(IEnumerable<Assembly> messagesAssembly);

    ICQRSBuilder SubscribeAllCommands(IEnumerable<Assembly> messagesAssembly);

    ICQRSBuilder SubscribeAllEvents(IEnumerable<Assembly> messagesAssembly);
  }
  internal class CQRSBuilder : ICQRSBuilder
  {
    private readonly ISubscriber _subscriber;
    public CQRSBuilder(ISubscriber subscriber)
    {
      _subscriber = subscriber;
    }

    public ICQRSBuilder SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
                Func<TCommand, MiddlinkException, IRejectedEvent> onError = null)
                where TCommand : ICommand
    {
      _subscriber.SubscribeCommand<TCommand>(@namespace, queueName, onError);
      return this;
    }

    public ICQRSBuilder SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
                Func<TEvent, MiddlinkException, IRejectedEvent> onError = null)
                where TEvent : IDomainEvent
    {
      _subscriber.SubscribeEvent<TEvent>(@namespace, queueName, onError);
      return this;
    }
    public ICQRSBuilder SubscribeAllMessages(IEnumerable<Assembly> messagesAssembly)
    {
      _subscriber.SubscribeAllMessages(messagesAssembly);
      return this;
    }

    public ICQRSBuilder SubscribeAllCommands(IEnumerable<Assembly> messagesAssembly)
    {
      _subscriber.SubscribeAllCommands(messagesAssembly);
      return this;
    }

    public ICQRSBuilder SubscribeAllEvents(IEnumerable<Assembly> messagesAssembly)
    {
      _subscriber.SubscribeAllEvents(messagesAssembly);
      return this;
    }
  }

  public static class HandlerExtensions
  {
    public static ISubscriber UseCQRS(this IApplicationBuilder applicationBuilder, Action<ICQRSBuilder> cqrsBuilder = null)
    {
      var subscriber = new Subscriber(applicationBuilder.ApplicationServices);
      if (cqrsBuilder != null)
      {
        cqrsBuilder(new CQRSBuilder(subscriber));
      }
      return subscriber;
    }
    public static void AddCQRS(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
    {
      foreach (var assembly in assemblies)
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
