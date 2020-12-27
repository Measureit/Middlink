using System;
using System.Collections.Generic;
using System.Linq;

namespace Middlink.Core.CQRS.Collections
{
    public class MetadataCollection : Dictionary<string, object>, IMetadataCollection
    {
        public static MetadataCollection Empty => new MetadataCollection();

        public MetadataCollection(IEnumerable<KeyValuePair<string, object>> keyValuePairs) : base(keyValuePairs.ToDictionary(e => e.Key, e => e.Value))
        {
        }

        private MetadataCollection()
        {
        }

        public object GetValue(string key) => this[key];

        public T GetValue<T>(string key, Func<object, T> converter)
        {
            var value = this[key];
            return converter(value);
        }

        public IMetadataCollection Merge(IMetadataCollection metadata)
        {
            var dict = this.ToDictionary(e => e.Key, e => e.Value);
            return new MetadataCollection(metadata.Union(dict));
        }
    }
}
