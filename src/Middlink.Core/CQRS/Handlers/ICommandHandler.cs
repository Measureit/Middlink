using Middlink.Core.CQRS.Commands;
using System.Threading.Tasks;

namespace Middlink.Core.CQRS.Handlers
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext context);
    }
}
