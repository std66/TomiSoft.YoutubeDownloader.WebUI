using System;

namespace TomiSoft.Common.SystemClock {
    public interface ITimerFactory {
        ITimer CreateTimer(int intervalInMilliseconds);
        IDisposable ScheduleExecution(int intervalInMilliseconds, bool runOnce, Action action);
    }
}
