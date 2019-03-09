using System;
using System.Collections.Generic;

namespace TomiSoft.Common.Caching {
    public interface ICache<T> {
        event EventHandler<T> TtlExpired;
        IEnumerable<string> Keys { get; }
        void Set(string key, T objectToStore, TimeSpan ttl);
        void Set(string key, T objectToStore, TimeSpan ttl, Action<T> onTtlExpiration);
        T Get(string key);
        bool TryGet(string key, out T result);
        bool HasItemWithKey(string key);
    }
}
