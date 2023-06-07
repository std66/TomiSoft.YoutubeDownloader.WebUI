using System.Collections.Generic;
using TomiSoft.Common.SystemProcess;
using TomiSoft.Common.SystemProcess.CliWrap;

namespace TomiSoft.YoutubeDownloader {
	public class ProcessFactory : IProcessFactory {
		private readonly string ExecutablePath;

		public ProcessFactory(IYoutubeDlConfiguration configuration) {
			this.ExecutablePath = configuration.ExecutablePath;
		}

		public IProcess Create(params string[] args) {
			return new CapturingProcess(this.ExecutablePath, args);
		}

		public IProcess Create(IEnumerable<string> args) {
			return new CapturingProcess(this.ExecutablePath, args);
		}

		public IAsyncProcess CreateAsyncProcess() {
			return new CliWrapProcess(this.ExecutablePath);
		}
	}
}