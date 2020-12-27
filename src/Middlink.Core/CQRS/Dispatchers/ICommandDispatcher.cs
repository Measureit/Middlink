using Middlink.Core.CQRS.Commands;
using System.Threading.Tasks;

namespace Middlink.Core.CQRS.Dispatchers
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command, ICorrelationContext context) where TCommand : ICommand;
    }
}
