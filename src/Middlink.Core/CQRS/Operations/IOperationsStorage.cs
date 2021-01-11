using System;
using System.Threading.Tasks;

namespace Middlink.Core.CQRS.Operations
{
    public interface IOperationsStorage
    {
        Task<OperationDto> GetAsync(Guid id);

        Task SetAsync(Guid id, string userId, string name, OperationState state,
            string resource, Guid resourceId, string code = null, string reason = null);
    }
}
