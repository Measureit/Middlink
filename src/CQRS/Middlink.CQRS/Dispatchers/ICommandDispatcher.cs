using Middlink.Commands;
using System.Threading.Tasks;

namespace Middlink.CQRS.Dispatchers
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command, ICorrelationContext context) where TCommand : ICommand;
    }
}
