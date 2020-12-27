using Middlink.Core;
using System.Threading.Tasks;

namespace Middlink.CQRS.Operations.SignalR.Hubs
{
    public interface IOperationBroadcaster
    {
        Task operationPending(IMessage status);
        Task operationRejected(IMessage status);
        Task operationCompleted(IMessage status);
    }
}
