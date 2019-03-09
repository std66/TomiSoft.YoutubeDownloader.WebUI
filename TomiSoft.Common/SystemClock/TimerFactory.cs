using System;

namespace TomiSoft.Common.SystemClock {
    public class TimerFactory : ITimerFactory {
        public IDisposable ScheduleExecution(int intervalInMilliseconds, bool runOnce, Action action) {
            return new TimedEvent(this, intervalInMilliseconds, action, runOnce);
        }

        public ITimer CreateTimer(int intervalInMilliseconds) {
            return new DotNetTimer(intervalInMilliseconds);
        }
    }
}
