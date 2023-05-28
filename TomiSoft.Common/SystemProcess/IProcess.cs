using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TomiSoft.Common.SystemProcess {
	public interface IProcess : IDisposable {
		int ExitCode { get; }
		bool ExitedSuccessfully { get; }
		IReadOnlyList<string> StandardOutputLines { get; }
		IReadOnlyList<string> StandardErrorLines { get; }
		string CommandLine { get; }

		event DataReceivedEventHandler OutputDataReceived;
		event DataReceivedEventHandler ErrorDataReceived;
		event EventHandler Exited;

		void Start();
		void WaitForExit();
		string GetOutputAsString();
		string GetErrorAsString();
	}
}