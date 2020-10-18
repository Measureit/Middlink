using System;
using System.Threading.Tasks;
using Middlink.CQRS.Operations.Dto;
using Middlink.CQRS.Operations.Operations;

namespace Middlink.CQRS.Operations.Services.Operations
{
    public interface IOperationsStorage
    {
        Task<OperationDto> GetAsync(Guid id);

        Task SetAsync(Guid id, Guid userId, string name, OperationState state,
            string resource, Guid resourceId, string code = null, string reason = null);
    }
}
