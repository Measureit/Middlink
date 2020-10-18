using Microsoft.AspNetCore.Builder;
using Middlink.MessageBus.MessageBrokers;
using Middlink.MessageBus.Services;
using System;

namespace Middlink.Notifications.SignalR.Extensions
{
    public static class Extensions
    {
        public static string ToUserGroup(this Guid userId) => userId.ToString().ToUserGroup();

        public static string ToUserGroup(this string userId) => $"users:{userId}";
    }
}
