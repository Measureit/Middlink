using Microsoft.Extensions.Caching.Memory;
using Middlink.Dto;
using Middlink.Services;
using System;
using System.Threading.Tasks;

namespace Middlink.MVC.Services.Operations
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
