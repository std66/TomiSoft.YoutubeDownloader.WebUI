using System;
using TomiSoft.Common.SystemClock;

namespace TomiSoft.Common.Caching {
    internal class CacheEntry<T> {
        public DateTime CreateTime { get; }
        public T Object { get; }
        public TimeSpan Ttl { get; }
        public Action<T> OnTtlExpiration { get; }

        internal CacheEntry(T obj, TimeSpan ttl, Action<T> onTtlExpiration, DateTime createTime) {
            Object = obj;
            Ttl = ttl;
            OnTtlExpiration = onTtlExpiration;
            CreateTime = createTime;
        }

        internal bool IsExpired(ISystemClock clock) {
            return this.CreateTime.Add(this.Ttl) < clock.UtcNow;
        }
    }
}
