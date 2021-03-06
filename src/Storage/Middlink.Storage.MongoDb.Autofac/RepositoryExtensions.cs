﻿using Autofac;
using Microsoft.Extensions.Configuration;
using Middlink.Core.Storage;
using Middlink.Extensions;
using MongoDB.Driver;
using System;

namespace Middlink.Storage.MongoDb.Autofac
{
    //to  można wykorzystać bardziej generycznie (nie tylko dla mongo)
    public interface IRepositoryBuilder
    {
        IRepositoryBuilder AddRepository<TEntity>(string collectionName)
            where TEntity : IIdentifiable;
    }
    internal class MongoRepositoryBuilder : IRepositoryBuilder
    {
        private readonly ContainerBuilder _containerBuilder;
        public MongoRepositoryBuilder(ContainerBuilder builder)
        {
            _containerBuilder = builder;
        }

        public IRepositoryBuilder AddRepository<TEntity>(string collectionName) where TEntity : IIdentifiable
        {
            RepositoryExtensions.AddMongoRepository<TEntity>(_containerBuilder, collectionName);
            return this;
        }
    }
    public static class RepositoryExtensions
    {
        public static void AddMongo(this ContainerBuilder builder, Action<IRepositoryBuilder> repositoryBuilderAction = null)
        {
            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var options = configuration.GetOptions<MongoDbOptions>("mongo");

                return options;
            }).SingleInstance();

            builder.Register(context =>
            {
                var options = context.Resolve<MongoDbOptions>();

                return new MongoClient(options.ConnectionString);
            }).SingleInstance();

            builder.Register(context =>
            {
                var options = context.Resolve<MongoDbOptions>();
                var client = context.Resolve<MongoClient>();
                return client.GetDatabase(options.Database);

            }).InstancePerLifetimeScope();

            builder.RegisterType<MongoDbInitializer>()
                .As<IInitializer>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MongoDbSeeder>()
                .As<IMongoDbSeeder>()
                .InstancePerLifetimeScope();

            if (repositoryBuilderAction != null)
            {
                repositoryBuilderAction(new MongoRepositoryBuilder(builder));
            }
        }

        public static void AddMongoRepository<TEntity>(this ContainerBuilder builder, string collectionName) where TEntity : IIdentifiable
            => builder.Register(ctx => new MongoRepository<TEntity>(ctx.Resolve<IMongoDatabase>(), collectionName))
                .As<IRepository<TEntity>>()
                .InstancePerLifetimeScope();
    }
}
