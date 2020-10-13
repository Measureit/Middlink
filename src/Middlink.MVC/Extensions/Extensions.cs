using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Middlink.Dispatchers;
using Middlink.Handlers;
using Middlink.Services;
using System;
using System.Reflection;
using Middlink.MVC.Services.MessageBrokers;

namespace Middlink.MVC.Extensions
{
    public static class Extensions
    {
    public static string ToUserGroup(this Guid userId) => userId.ToString().ToUserGroup();

    public static string ToUserGroup(this string userId) => $"users:{userId}";
    public static IBusSubscriber UseInMemoryQueue(this IApplicationBuilder app) => new BusSubscriber(app);
    }
}
