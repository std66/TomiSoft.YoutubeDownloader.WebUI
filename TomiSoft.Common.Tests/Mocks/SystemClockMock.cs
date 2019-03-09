using System;
using TomiSoft.Common.SystemClock;

namespace TomiSoft.Common.Tests.Mocks {
    class SystemClockMock : ISystemClock {
        public DateTime UtcNow { get; set; }
    }
}
