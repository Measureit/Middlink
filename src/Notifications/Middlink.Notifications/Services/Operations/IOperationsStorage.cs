using Middlink.Notifications.Dto;
using Middlink.Notifications.Operations;
using System;
using System.Threading.Tasks;

namespace Middlink.Notifications.Services.Operations
{
    public interface IOperationsStorage
    {
        Task<OperationDto> GetAsync(Guid id);

        Task SetAsync(Guid id, Guid userId, string name, OperationState state,
            string resource, Guid resourceId, string code = null, string reason = null);
    }
}
