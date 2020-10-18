using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Middlink.CQRS.Operations.Dto;
using Middlink.CQRS.Operations.Operations;
using Middlink.CQRS.Operations.Services.Operations;

namespace Middlink.CQRS.Operations.SignalR.Services.Operations
{
    public class OperationsStorage : IOperationsStorage
    {
        private readonly MemoryCache _cache;

        public OperationsStorage()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
        public Task<OperationDto> GetAsync(Guid id)
        {
            _cache.TryGetValue(id, out OperationDto result);
            return Task.FromResult(result);
        }

        public async Task SetAsync(Guid id, Guid userId, string name, OperationState state, string resource, Guid resourceId, string code = null, string reason = null)
        {
            var newState = state.ToString().ToLowerInvariant();
            var operation = await GetAsync(id);
            if (operation == null)
            {
                operation = new OperationDto();
                _cache.Set(id, operation, TimeSpan.FromMinutes(1));
            }
            operation.Id = id;
            operation.UserId = userId;
            operation.Name = name;
            operation.State = newState;
            operation.Resource = resource;
            operation.ResourceId = resourceId;
            operation.Code = code ?? string.Empty;
            operation.Reason = reason ?? string.Empty;
        }
    }
}
