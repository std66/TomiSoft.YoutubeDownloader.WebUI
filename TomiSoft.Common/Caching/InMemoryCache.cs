using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TomiSoft.Common.SystemClock;

namespace TomiSoft.Common.Caching {
    public class InMemoryCache<T> : ICache<T>, IDisposable {
        private readonly IDictionary<string, CacheEntry<T>> storage = new ConcurrentDictionary<string, CacheEntry<T>>();
        private readonly ISystemClock clock;
        private readonly IDisposable expirationCheckTimedEvent;

        public IEnumerable<string> Keys => storage.Keys;

        public event EventHandler<T> TtlExpired;

        public InMemoryCache(ISystemClock systemClock, ITimerFactory timerFactory) {
            this.clock = systemClock;
            this.expirationCheckTimedEvent = timerFactory.ScheduleExecution(1000, false, this.CheckExpiration);
        }
        
        public T Get(string key) {
            return storage[key].Object;
        }

        public void Set(string key, T objectToStore, TimeSpan ttl) {
            Set(key, objectToStore, ttl, null);
        }

        public void Set(string key, T objectToStore, TimeSpan ttl, Action<T> onTtlExpiration) {
            storage.Add(
                key,
                new CacheEntry<T>(objectToStore, ttl, onTtlExpiration, this.clock.UtcNow)
            );
        }

        public bool TryGet(string key, out T result) {
            result = default(T);

            if (storage.TryGetValue(key, out var x)) {
                result = x.Object;
                return true;
            }

            return false;
        }

        public bool HasItemWithKey(string key) {
            return storage.ContainsKey(key);
        }

        private void CheckExpiration() {
            List<string> keysToRemove = new List<string>();

            foreach (var item in this.storage) {
                CacheEntry<T> entry = item.Value;
                if (entry.IsExpired(this.clock)) {
                    keysToRemove.Add(item.Key);

                    entry.OnTtlExpiration?.Invoke(entry.Object);
                    this.TtlExpired?.Invoke(this, entry.Object);
                }
            }

            foreach (string key in keysToRemove) {
                this.storage.Remove(key);
            }
        }

        public void Dispose() {
            this.expirationCheckTimedEvent.Dispose();
        }
    }
}
