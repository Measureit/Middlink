using System;

namespace Middlink.Notifications.SignalR.Extensions
{
    public static class Extensions
    {
        public static string ToUserGroup(this Guid userId) => userId.ToString().ToUserGroup();

        public static string ToUserGroup(this string userId) => $"users:{userId}";
        //public static IBusSubscriber UseInMemoryQueue(this IApplicationBuilder app) => new BusSubscriber(app);
    }
}
