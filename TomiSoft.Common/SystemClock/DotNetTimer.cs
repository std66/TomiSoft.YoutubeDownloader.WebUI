using System;
using System.Timers;

namespace TomiSoft.Common.SystemClock {
    internal class DotNetTimer : ITimer {
        private readonly Timer timer;

        public event EventHandler Elapsed;

        public void Dispose() {
            this.timer.Elapsed -= this.Timer_Elapsed;
            this.timer.Dispose();
        }

        internal DotNetTimer(int intervalInMilliseconds) {
            this.timer = new Timer(1000);
            this.timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            this.Elapsed?.Invoke(this, EventArgs.Empty);
        }

        public void Start() {
            this.timer.Start();
        }

        public void Stop() {
            this.timer.Stop();
        }
    }
}
