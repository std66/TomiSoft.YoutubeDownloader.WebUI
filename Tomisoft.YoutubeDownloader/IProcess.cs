using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tomisoft.YoutubeDownloader {
    public interface IProcess : IDisposable {
        int ExitCode { get; }
        bool ExitedSuccessfully { get; }
        IReadOnlyList<string> StandardOutputLines { get; }
        IReadOnlyList<string> StandardErrorLines { get; }

        event DataReceivedEventHandler OutputDataReceived;
        event DataReceivedEventHandler ErrorDataReceived;
        event EventHandler Exited;

        void Start();
        void WaitForExit();
        string GetOutputAsString();
    }
}
