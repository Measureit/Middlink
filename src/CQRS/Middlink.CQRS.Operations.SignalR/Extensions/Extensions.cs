using System;

namespace Middlink.CQRS.Operations.SignalR.Extensions
{
    public static class Extensions
    {
        public static string ToUserGroup(this Guid userId) => userId.ToString().ToUserGroup();

        public static string ToUserGroup(this string userId) => $"users:{userId}";

        public static string ToAnonymousSessionGroup(this Guid anonymousSessionId) => anonymousSessionId.ToString().ToAnonymousSessionGroup();

        public static string ToAnonymousSessionGroup(this string anonymousSessionId) => $"anonymoussessions:{anonymousSessionId}";

    }
}
