using System;
using System.Collections.Generic;

namespace Middlink.Core.CQRS.Collections
{
    public interface IMetadataCollection : IReadOnlyDictionary<string, object>
    {
        object GetValue(string key);

        T GetValue<T>(string key, Func<object, T> converter);

        IMetadataCollection Merge(IMetadataCollection metadata);
    }
}
