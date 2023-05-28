using System;
using System.Collections.Generic;
using System.Linq;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks {
	abstract class BaseAudioDownloadMock : SimpleTextConsoleAppMock {
		private readonly string downloadPath;

		protected override bool CheckCmdLineArgs(IEnumerable<string> args) {
			string[] arr = args.ToArray();
			return arr[0] == "--newline" && arr[1] == "--extract-audio" && arr[2] == "--audio-format" && arr[3] == "mp3" && arr[4] == "-o" && this.IsValidOutputFilename(arr[5]) && Uri.IsWellFormedUriString(arr[6], UriKind.Absolute);
		}

		public BaseAudioDownloadMock(IEnumerable<KeyValuePair<WriteTo, string>> Data, int ReturnedExitCode, Behavior behavior, string downloadPath)
			: base(Data, ReturnedExitCode, behavior) {
			this.downloadPath = downloadPath;
		}

		private bool IsValidOutputFilename(string Filename) {
			string[] parts = Filename.Substring(downloadPath.Length + 1).Split('.');
			return Guid.TryParse(parts[0], out _) && parts[1] == "%(ext)s";
		}
	}
}