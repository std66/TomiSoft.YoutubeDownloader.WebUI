using System;
using System.Collections.Generic;

namespace TomiSoft.Common.SystemProcess {
	public class ProcessExecutionResult {
		public ProcessExecutionResult(int exitCode, string stdOut, string stdErr) {
			this.ExitCode = exitCode;
			this.StdOut = stdOut;
			this.StdErr = stdErr;
		}

		public int ExitCode { get; }
		public bool ExitedSuccessfully => this.ExitCode == 0;
		public string StdOut { get; }
		public string StdErr { get; }
		public IReadOnlyList<string> StdErrLines => this.StdErr.Split(Environment.NewLine.ToCharArray());
		public IReadOnlyList<string> StdOutLines => this.StdOut.Split(Environment.NewLine.ToCharArray());
	}
}