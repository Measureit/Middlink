using Microsoft.AspNetCore.Builder;
using System;
using Middlink.MVC.Services.MessageBrokers;
using Middlink.MessageBus.Services;

namespace Middlink.MVC.Extensions
{
  public static class Extensions
    {
    public static string ToUserGroup(this Guid userId) => userId.ToString().ToUserGroup();

    public static string ToUserGroup(this string userId) => $"users:{userId}";
    public static IBusSubscriber UseInMemoryQueue(this IApplicationBuilder app) => new BusSubscriber(app);
    }
}
