using Middlink.Dto;
using Middlink.Messages.Operations;
using System;
using System.Threading.Tasks;

namespace Middlink.Services
{
    public interface IOperationsStorage
    {
        Task<OperationDto> GetAsync(Guid id);

        Task SetAsync(Guid id, Guid userId, string name, OperationState state,
            string resource, Guid resourceId, string code = null, string reason = null);
    }
}
