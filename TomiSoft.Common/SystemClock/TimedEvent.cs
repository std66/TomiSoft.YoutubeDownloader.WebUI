using System;

namespace TomiSoft.Common.SystemClock {
    internal class TimedEvent : IDisposable {
        private readonly ITimer timer;
        private readonly Action action;
        private readonly bool runOnce;
        
        public TimedEvent(ITimerFactory timer, int intervalInMilliseconds, Action action, bool runOnce) {
            this.timer = timer.CreateTimer(intervalInMilliseconds);
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.runOnce = runOnce;

            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();
        }

        private void Timer_Elapsed(object sender, EventArgs e) {
            if (this.runOnce) {
                this.timer.Stop();
            }

            action();
        }

        public void Dispose() {
            this.timer.Stop();
            this.timer.Elapsed -= Timer_Elapsed;
            this.timer.Dispose();
        }
    }
}
