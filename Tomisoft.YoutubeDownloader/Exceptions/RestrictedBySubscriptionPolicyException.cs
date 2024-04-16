using System;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader.Exceptions {
	public class RestrictedBySubscriptionPolicyException : AccessToMediaRequiresLoginException {
		public RestrictedBySubscriptionPolicyException(ProcessExecutionResult Process, Uri MediaUri) : base(Process, MediaUri) {
		}
	}
}
