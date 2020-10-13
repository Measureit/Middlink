using Middlink.Messages;
using Middlink.Messages.Commands;
using System.Threading.Tasks;

namespace Middlink.Dispatchers
{
  public interface ICommandDispatcher
  {
    Task DispatchAsync<TCommand>(TCommand command, ICorrelationContext context) where TCommand : ICommand;
  }
}
