using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Middlink.Handlers
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        ValueTask<TResult> HandleAsync(TQuery query);
    }
}
