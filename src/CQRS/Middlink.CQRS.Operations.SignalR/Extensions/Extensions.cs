namespace Middlink.CQRS.Operations.SignalR.Extensions
{
    public static class Extensions
    {
        public static string ToUserGroup(this Guid userId) => userId.ToString().ToUserGroup();

        public static string ToUserGroup(this string userId) => $"users:{userId}";
    }
}
