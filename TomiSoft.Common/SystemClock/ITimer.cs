using System;

namespace TomiSoft.Common.SystemClock {
    public interface ITimer : IDisposable {
        event EventHandler Elapsed;
        void Start();
        void Stop();
    }
}
