using CliWrap;
using CliWrap.Buffered;
using System;
using System.Threading.Tasks;

namespace TomiSoft.Common.SystemProcess.CliWrap {
	public class CliWrapProcess : IAsyncProcess {
		private static char[] Newline = Environment.NewLine.ToCharArray();
		private readonly string executablePath;

		public CliWrapProcess(string executablePath) {
			this.executablePath = executablePath;
		}

		public async Task<ProcessExecutionResult> StartAsync(params string[] args) {
			Command cmd = Cli.Wrap(executablePath)
				.WithArguments(args);

			BufferedCommandResult result = await cmd.ExecuteBufferedAsync();

			return MapResult(result);
		}

		private ProcessExecutionResult MapResult(BufferedCommandResult result) {
			return new ProcessExecutionResult(
				result.ExitCode,
				result.StandardOutput.Split(Newline),
				result.StandardError.Split(Newline)
			);
		}
	}
}
