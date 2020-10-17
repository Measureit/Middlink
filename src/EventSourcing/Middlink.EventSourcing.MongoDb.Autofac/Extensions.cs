﻿using Autofac;
using System;

namespace Middlink.EventSourcing.MongoDb.Autofac
{
    public static class Extensions
    {
        public static void AddEventStore(this ContainerBuilder builder)
        {
            builder.RegisterType<MongoEventStore>().As<IEventStore>();
            builder.RegisterType<Session>().As<ISession>().InstancePerDependency();
        }
    }
}
