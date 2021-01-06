using Microsoft.AspNetCore.Mvc;
using Middlink.Core;
using Middlink.Core.CQRS.Commands;
using Middlink.Core.CQRS.Dispatchers;
using Middlink.Core.CQRS.Events;
using Middlink.Core.CQRS.Queries;
using Middlink.Core.MessageBus;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Middlink.MVC.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private static readonly string AcceptLanguageHeader = "accept-language";
        private static readonly string OperationHeader = "X-Operation";
        private static readonly string ResourceHeader = "X-Resource";
        private static readonly string ResourceIdHeader = "X-ResourceId";
        private static readonly string DefaultCulture = "en-us";
        private static readonly string PageLink = "page";
        private readonly IPublisher _publisher;
        private readonly IQueryDispatcher _queryDispatcher;

        public BaseController(
            IPublisher publisher,
            IQueryDispatcher queryDispatcher)
        {
            _publisher = publisher;
            _queryDispatcher = queryDispatcher;
        }

        protected ActionResult<T> Single<T>(T model, Func<T, bool> criteria = null)
        {
            if (model == null)
            {
                return NotFound();
            }
            var isValid = criteria == null || criteria(model);
            if (isValid)
            {
                return Ok(model);
            }

            return NotFound();
        }

        protected async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query) => await _queryDispatcher.QueryAsync<TResult>(query);

        protected ActionResult<PagedResult<T>> Collection<T>(PagedResult<T> pagedResult, Func<PagedResult<T>, bool> criteria = null)
        {
            if (pagedResult == null)
            {
                return NotFound();
            }
            var isValid = criteria == null || criteria(pagedResult);
            if (!isValid)
            {
                return NotFound();
            }
            if (!pagedResult.IsEmpty)
            {
                Response.Headers.Add("Link", GetLinkHeader(pagedResult));
                Response.Headers.Add("X-Total-Count", pagedResult.TotalResults.ToString());
            }

            return Ok(pagedResult);
        }

        protected async Task<IActionResult> SendAsync<T>(T command,
            Guid? resourceId = null, string resource = "") where T : ICommand
        {
            var context = GetContext<T>(resourceId, resource);
            await _publisher.SendAsync(command, context);

            return Accepted(context);
        }

        protected async Task<IActionResult> PublishAsync<T>(T @event,
            Guid? resourceId = null, string resource = "") where T : IDomainEvent
        {
            var context = await GetContextAsync<T>(resourceId, resource);

            await _publisher.PublishAsync(@event, context);

            return Ok(context);
        }

        protected IActionResult Accepted(ICorrelationContext context)
        {
            Response.Headers.Add(OperationHeader, $"{context.Id}");
            if (!string.IsNullOrWhiteSpace(context.Resource))
            {
                Response.Headers.Add(ResourceHeader, context.Resource);
            }
            if (context.ResourceId != Guid.Empty)
            {
                Response.Headers.Add(ResourceIdHeader, context.ResourceId.ToString());
            }

            return base.Accepted();
        }

        protected ICorrelationContext GetContext<T>(Guid? resourceId = null, string resource = "") where T : ICommand
        {
            if (!string.IsNullOrWhiteSpace(resource))
            {
                resource = $"{resource}/{resourceId}";
            }

            return CorrelationContext.Create<T>(Guid.NewGuid(), UserId, resourceId ?? Guid.Empty,
               HttpContext.TraceIdentifier, HttpContext.Connection.Id,
               Request.Path.ToString(), Culture, resource);
        }

        protected Task<ICorrelationContext> GetContextAsync<T>(Guid? resourceId = null, string resource = "") where T : IMessage
        {
            var (identityClaim, issuerClaim) = GetIdentityClaims();

            if (!string.IsNullOrWhiteSpace(resource))
            {
                resource = $"{resource}/{resourceId}";
            }

            return Task.FromResult(CorrelationContext.Create<T>(Guid.NewGuid(), UserId, resourceId ?? Guid.Empty,
               HttpContext.TraceIdentifier, HttpContext.Connection.Id,
               Request.Path.ToString(), Culture, resource));
        }

        protected (string userIdentifier, string issuer) GetIdentityClaims()
        {
            var identityClaim = HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier"));
            var issuerClaim = HttpContext.User.Claims.First(claim => claim.Type.Contains("iss"));

            return (identityClaim.Value, issuerClaim.Value);
        }

        protected bool IsAdmin
            => User.IsInRole("admin");

        protected Guid UserId
            => string.IsNullOrWhiteSpace(User?.Identity?.Name) ?
                Guid.Empty :
                Guid.Parse(User.Identity.Name);

        protected string Culture
            => Request.Headers.ContainsKey(AcceptLanguageHeader) ?
                    Request.Headers[AcceptLanguageHeader].First().ToLowerInvariant() :
                    DefaultCulture;

        private string GetLinkHeader(PagedResultBase result)
        {
            var first = GetPageLink(result.CurrentPage, 1);
            var last = GetPageLink(result.CurrentPage, result.TotalPages);
            var prev = string.Empty;
            var next = string.Empty;
            if (result.CurrentPage > 1 && result.CurrentPage <= result.TotalPages)
            {
                prev = GetPageLink(result.CurrentPage, result.CurrentPage - 1);
            }
            if (result.CurrentPage < result.TotalPages)
            {
                next = GetPageLink(result.CurrentPage, result.CurrentPage + 1);
            }

            return $"{FormatLink(next, "next")}{FormatLink(last, "last")}" +
                   $"{FormatLink(first, "first")}{FormatLink(prev, "prev")}";
        }

        private string GetPageLink(int currentPage, int page)
        {
            var path = Request.Path.HasValue ? Request.Path.ToString() : string.Empty;
            var queryString = Request.QueryString.HasValue ? Request.QueryString.ToString() : string.Empty;
            var conjunction = string.IsNullOrWhiteSpace(queryString) ? "?" : "&";
            var fullPath = $"{path}{queryString}";
            var pageArg = $"{PageLink}={page}";
            var link = fullPath.Contains($"{PageLink}=")
                ? fullPath.Replace($"{PageLink}={currentPage}", pageArg)
                : fullPath += $"{conjunction}{pageArg}";

            return link;
        }

        private static string FormatLink(string path, string rel)
            => string.IsNullOrWhiteSpace(path) ? string.Empty : $"<{path}>; rel=\"{rel}\",";
    }
}
