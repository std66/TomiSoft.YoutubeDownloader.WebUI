using System.Collections.Generic;

namespace TomiSoft.Common.SystemProcess {
	public class ProcessExecutionResult {
		public ProcessExecutionResult(int exitCode, IReadOnlyList<string> stdOut, IReadOnlyList<string> stdErr) {
			this.ExitCode = exitCode;
			this.StdOut = stdOut;
			this.StdErr = stdErr;
		}

		public int ExitCode { get; }
		public bool ExitedSuccessfully => this.ExitCode == 0;
		public IReadOnlyList<string> StdOut { get; }
		public IReadOnlyList<string> StdErr { get; }
	}
}