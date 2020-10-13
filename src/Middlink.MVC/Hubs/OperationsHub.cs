using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Middlink.MVC.Extensions;
using Middlink.MVC.Hubs.Broadcasters;
using System.Threading.Tasks;

namespace Middlink.MVC.Hubs
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
