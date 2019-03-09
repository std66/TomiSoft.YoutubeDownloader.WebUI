using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TomiSoft.Common.Caching;
using TomiSoft.Common.Tests.Mocks;

namespace TomiSoft.Common.Tests {
    [TestClass]
    public class InMemoryCacheTests {
        [TestMethod]
        public void CanSetItem() {
            SystemClockMock clock = new SystemClockMock();
            TimerFactoryMock timerFactory = new TimerFactoryMock(
                timedEventMockFactory: (x, y, z) => new EmptyDisposable(),
                timerMockFactory: null
            );

            const string key = "test_key";
            const string value = "test_value";

            using (InMemoryCache<string> cache = new InMemoryCache<string>(clock, timerFactory)) {
                cache.Set(key, value, TimeSpan.FromTicks(1));
                Assert.IsTrue(cache.HasItemWithKey(key), $"Cache does not contain entry with key '{key}'.");
            }
        }

        [TestMethod]
        public void CanSetMultipleItems() {
            SystemClockMock clock = new SystemClockMock();
            TimerFactoryMock timerFactory = new TimerFactoryMock(
                timedEventMockFactory: (x, y, z) => new EmptyDisposable(),
                timerMockFactory: null
            );

            Dictionary<string, string> values = new Dictionary<string, string>() {
                ["key1"] = "value1",
                ["key2"] = "value2"
            };

            using (InMemoryCache<string> cache = new InMemoryCache<string>(clock, timerFactory)) {
                foreach (var item in values) {
                    cache.Set(item.Key, item.Value, TimeSpan.FromTicks(1));
                }

                foreach (var item in values) {
                    Assert.IsTrue(cache.HasItemWithKey(item.Key), $"Cache does not contain entry with key '{item.Key}'.");
                    Assert.AreEqual(item.Value, cache.Get(item.Key));
                }
            }
        }

        [TestMethod]
        public void CanGetItem() {
            SystemClockMock clock = new SystemClockMock();
            TimerFactoryMock timerFactory = new TimerFactoryMock(
                timedEventMockFactory: (x, y, z) => new EmptyDisposable(),
                timerMockFactory: null
            );

            const string key = "test_key";
            const string value = "test_value";

            using (InMemoryCache<string> cache = new InMemoryCache<string>(clock, timerFactory)) {
                cache.Set(key, value, TimeSpan.FromTicks(1));
                string actual = cache.Get(key);

                Assert.AreEqual(value, actual);
            }
        }

        [TestMethod]
        public void CanGetItemBeforeExpiration() {
            TimedEventMock timedEventMock = null;

            SystemClockMock clock = new SystemClockMock();
            TimerFactoryMock timerFactory = new TimerFactoryMock(
                timedEventMockFactory: (interval, runOnce, action) => {
                    TimedEventMock result = new TimedEventMock(interval, runOnce, action);
                    timedEventMock = result;

                    return result;
                },
                timerMockFactory: null
            );

            const string key = "test_key";
            const string value = "test_value";

            clock.UtcNow = DateTime.Now;
            TimeSpan expiration = TimeSpan.FromHours(1);

            using (InMemoryCache<string> cache = new InMemoryCache<string>(clock, timerFactory)) {
                cache.Set(key, value, expiration);

                clock.UtcNow += expiration - TimeSpan.FromMinutes(30);
                timedEventMock.Tick();

                Assert.IsTrue(cache.HasItemWithKey(key), $"Cache does not contain entry with key '{key}'.");

                string actual = cache.Get(key);

                Assert.AreEqual(value, actual);
            }
        }

        [TestMethod]
        public void CanNotGetItemAfterExpiration() {
            TimedEventMock timedEventMock = null;

            SystemClockMock clock = new SystemClockMock();
            TimerFactoryMock timerFactory = new TimerFactoryMock(
                timedEventMockFactory: (interval, runOnce, action) => {
                    TimedEventMock result = new TimedEventMock(interval, runOnce, action);
                    timedEventMock = result;

                    return result;
                },
                timerMockFactory: null
            );

            const string key = "test_key";
            const string value = "test_value";

            clock.UtcNow = DateTime.Now;
            TimeSpan expiration = TimeSpan.FromHours(1);

            using (InMemoryCache<string> cache = new InMemoryCache<string>(clock, timerFactory)) {
                cache.Set(key, value, expiration);

                clock.UtcNow += expiration + TimeSpan.FromSeconds(5);
                timedEventMock.Tick();

                Assert.IsFalse(cache.HasItemWithKey(key), $"Cache contains entry with key '{key}'.");
                Assert.ThrowsException<KeyNotFoundException>(() => cache.Get(key));
            }
        }

        [TestMethod]
        public void CanInvokeExpirationMethodWhenItemExpires() {
            TimedEventMock timedEventMock = null;

            SystemClockMock clock = new SystemClockMock();
            TimerFactoryMock timerFactory = new TimerFactoryMock(
                timedEventMockFactory: (interval, runOnce, action) => {
                    TimedEventMock result = new TimedEventMock(interval, runOnce, action);
                    timedEventMock = result;

                    return result;
                },
                timerMockFactory: null
            );

            const string key = "test_key";
            const string value = "test_value";

            clock.UtcNow = DateTime.Now;
            TimeSpan expiration = TimeSpan.FromHours(1);

            bool expirationMethodInvoked = false;
            bool expirationEventIsFiredForItem = false;

            using (InMemoryCache<string> cache = new InMemoryCache<string>(clock, timerFactory)) {
                cache.TtlExpired += (o, expiredValue) => {
                    if (expiredValue == value)
                        expirationEventIsFiredForItem = true;
                };

                cache.Set(key, value, expiration, (x) => { expirationMethodInvoked = true; });

                clock.UtcNow += expiration + TimeSpan.FromSeconds(5);
                timedEventMock.Tick();

                Assert.IsTrue(expirationEventIsFiredForItem, $"{nameof(cache.TtlExpired)} is not fired for the expired item.");
                Assert.IsTrue(expirationMethodInvoked, "The method set to be execute on expiration of the item has not invoked.");
            }
        }

        [TestMethod]
        public void CanDisposeProperly() {
            TimedEventMock timedEventMock = null;

            SystemClockMock clock = new SystemClockMock();
            TimerFactoryMock timerFactory = new TimerFactoryMock(
                timedEventMockFactory: (interval, runOnce, action) => {
                    TimedEventMock result = new TimedEventMock(interval, runOnce, action);
                    timedEventMock = result;

                    return result;
                },
                timerMockFactory: null
            );
            
            using (InMemoryCache<string> cache = new InMemoryCache<string>(clock, timerFactory)) {
                //do nothing here
            }

            Assert.IsTrue(timedEventMock.DisposeInvoked, $"Dispose method is not invoked on {nameof(timedEventMock)}.");
        }
    }
}
