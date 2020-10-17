using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Middlink.Notifications.Hubs.Broadcasters;
using Middlink.Notifications.SignalR.Extensions;
using System.Threading.Tasks;

namespace Middlink.Notifications.SignalR.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OperationsHub : Hub<IOperationBroadcaster>
    {
        public Task SubscribeAsync()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name.ToUserGroup());
        }

        public Task UnsubscribeAsync()
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name.ToUserGroup());
        }
    }
}
