using System;
using TomiSoft.Common.SystemClock;

namespace TomiSoft.Common.Tests.Mocks {
    class TimerFactoryMock : ITimerFactory {
        private readonly Func<int, bool, Action, IDisposable> timedEventMockFactory;
        private readonly Func<int, ITimer> timerMockFactory;

        public event EventHandler<IDisposable> ExecutionScheduled;
        public event EventHandler<ITimer> TimerCreated;

        public TimerFactoryMock(Func<int, bool, Action, IDisposable> timedEventMockFactory, Func<int, ITimer> timerMockFactory) {
            this.timedEventMockFactory = timedEventMockFactory;
            this.timerMockFactory = timerMockFactory;
        }

        public ITimer CreateTimer(int intervalInMilliseconds) {
            ITimer result = timerMockFactory(intervalInMilliseconds);
            this.TimerCreated?.Invoke(this, result);
            return result;
        }

        public IDisposable ScheduleExecution(int intervalInMilliseconds, bool runOnce, Action action) {
            IDisposable result = timedEventMockFactory(intervalInMilliseconds, runOnce, action);
            this.ExecutionScheduled?.Invoke(this, result);
            return result;
        }
    }
}
