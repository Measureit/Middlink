using System.Threading.Tasks;

namespace Middlink.CQRS.Operations.Hubs.Broadcasters
{
    public interface IOperationBroadcaster
    {
        Task operationPending(IMessage status);
        Task operationRejected(IMessage status);
        Task operationCompleted(IMessage status);
    }
}
