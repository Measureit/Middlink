using Middlink.Messages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Middlink.Operations.Services
{
    public interface IOperationPublisher
    {
        Task PendingAsync(ICorrelationContext context);
        Task CompleteAsync(ICorrelationContext context);
        Task RejectAsync(ICorrelationContext context, string code, string message);
    }
}
