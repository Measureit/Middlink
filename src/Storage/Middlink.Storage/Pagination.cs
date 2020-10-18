using Middlink.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middlink.Storage
{
    public static class Pagination
    {
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IEnumerable<T> collection, PagedQueryBase query)
            => await collection.PaginateAsync(query.Page, query.Results);

        public static Task<PagedResult<T>> PaginateAsync<T>(this IEnumerable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var isEmpty = collection.Any() == false;
            if (isEmpty)
            {
                return Task.FromResult(PagedResult<T>.Empty);
            }
            var totalResults = collection.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var data = collection.Limit(page, resultsPerPage).ToList();

            return Task.FromResult(PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults));
        }

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> collection, PagedQueryBase query)
            => collection.Limit(query.Page, query.Results);

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var skip = (page - 1) * resultsPerPage;
            var data = collection.Skip(skip)
                .Take(resultsPerPage);

            return data;
        }
    }
}
