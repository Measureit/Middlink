using Autofac;
using Microsoft.Extensions.Configuration;
using Middlink.Extensions;
using Middlink.Storage.Entities;
using MongoDB.Driver;
using System;

namespace Middlink.Storage.MongoDb.Autofac
{
    //public interface IRepositoryBuilder
    //{
        

    //}
    //internal class RepositoryBuilder : IRepositoryBuilder
    //{
    //    public RepositoryBuilder(ContainerBuilder builder)
    //    {
    //        ContainerBuilder = builder;
    //    }

    //    internal ContainerBuilder ContainerBuilder { get; private set; }
    //}
    public static class RepositoryExtensions
    {
        public static void AddMongo(this ContainerBuilder builder)
            //, Func<IRepositoryBuilder, IRepositoryBuilder> repositoryBuilderAction)
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

            //repositoryBuilderAction(new RepositoryBuilder(builder));
        }

        public static void AddMongoRepository<TEntity>(this ContainerBuilder builder, string collectionName) where TEntity : IIdentifiable
            => builder.Register(ctx => new MongoRepository<TEntity>(ctx.Resolve<IMongoDatabase>(), collectionName))
                .As<IRepository<TEntity>>()
                .InstancePerLifetimeScope();
    }
}
