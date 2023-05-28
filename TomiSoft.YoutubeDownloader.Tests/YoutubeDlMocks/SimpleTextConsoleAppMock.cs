using System.Collections.Generic;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks {
	abstract class SimpleTextConsoleAppMock : BaseProcessMock {
		private readonly IEnumerable<KeyValuePair<WriteTo, string>> Data;
		private readonly int ReturnedExitCode;

		public enum WriteTo {
			StdOut, StdErr
		}

		public SimpleTextConsoleAppMock(IEnumerable<KeyValuePair<WriteTo, string>> Data, int ReturnedExitCode, Behavior behavior) : base(behavior) {
			this.Data = Data;
			this.ReturnedExitCode = ReturnedExitCode;
		}

		protected override int Main(string[] args) {
			foreach (var line in this.Data) {
				switch (line.Key) {
					case WriteTo.StdOut:
						Write(line.Value);
						break;
					case WriteTo.StdErr:
						ErrorWrite(line.Value);
						break;
				}
			}

			return ReturnedExitCode;
		}
	}

	class To {
		public static KeyValuePair<SimpleTextConsoleAppMock.WriteTo, string> Out(string data) => new KeyValuePair<SimpleTextConsoleAppMock.WriteTo, string>(SimpleTextConsoleAppMock.WriteTo.StdOut, data);
		public static KeyValuePair<SimpleTextConsoleAppMock.WriteTo, string> Err(string data) => new KeyValuePair<SimpleTextConsoleAppMock.WriteTo, string>(SimpleTextConsoleAppMock.WriteTo.StdErr, data);
	}
}