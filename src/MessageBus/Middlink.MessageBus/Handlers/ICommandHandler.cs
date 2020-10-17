using Middlink.Messages;
using Middlink.Messages.Commands;
using System.Threading.Tasks;

namespace Middlink.MessageBus.Handlers
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext context);
    }
}
