using System;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader.Exceptions {
	public class MediaInformationExtractException : Exception {
		public Uri MediaUri { get; }
		public string StandardError { get; }
		public int ExitCode { get; }

		public MediaInformationExtractException(ProcessExecutionResult Process, Uri MediaUri)
			: base($"There was an error while getting media information. Media URI: {MediaUri} YoutubeDl stderr: {Process.StdErr}") {
			this.MediaUri = MediaUri;
			this.StandardError = Process.StdErr;
			this.ExitCode = Process.ExitCode;
		}
	}
}