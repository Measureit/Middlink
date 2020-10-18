using Middlink.Commands;
using System.Threading.Tasks;

namespace Middlink.CQRS.Handlers
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext context);
    }
}
