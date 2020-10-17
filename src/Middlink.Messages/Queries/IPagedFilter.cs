using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Messages.Queries
{
    public interface IPagedFilter<TResult, in TQuery> where TQuery : IQuery
    {
        PagedResult<TResult> Filter(IEnumerable<TResult> values, TQuery query);
    }
}
