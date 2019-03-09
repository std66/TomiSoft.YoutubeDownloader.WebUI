using System;

namespace TomiSoft.Common.Tests.Mocks {
    class TimedEventMock : IDisposable {
        public TimedEventMock(int intervalInMilliseconds, bool runOnce, Action action) {
            IntervalInMilliseconds = intervalInMilliseconds;
            RunOnce = runOnce;
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public int IntervalInMilliseconds { get; }
        public bool RunOnce { get; }
        public Action Action { get; }
        public bool DisposeInvoked { get; private set; }

        public void Tick() {
            Action();
        }

        public void Dispose() {
            this.DisposeInvoked = true;
        }
    }
}
