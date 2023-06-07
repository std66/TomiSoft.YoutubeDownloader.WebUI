using System;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader.Exceptions {
	public class PrivateMediaException : AccessToMediaRequiresLoginException {
		public PrivateMediaException(ProcessExecutionResult Process, Uri MediaUri) : base(Process, MediaUri) {
		}
	}
}