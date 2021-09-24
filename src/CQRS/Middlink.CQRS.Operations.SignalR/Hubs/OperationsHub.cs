using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Middlink.CQRS.Operations.SignalR.Extensions;
using System;

namespace Middlink.CQRS.Operations.SignalR.Hubs
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

        [AllowAnonymous]
        public Task SubscribeAnonymouseAsync(Guid sessionId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToAnonymousSessionGroup());
        }

        [AllowAnonymous]
        public Task UnsubscribeAnonymouseAsync(Guid sessionId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId.ToAnonymousSessionGroup());
        }
    }
}
