using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader.Tests.YoutubeDlMocks {
	internal class MockAsyncProcess : IAsyncProcess {
		public event Func<IReadOnlyList<string>, ProcessExecutionResult> StartAsyncInvoked;

		public MockAsyncProcess() {
		}

		public Task<ProcessExecutionResult> StartAsync(params string[] args) {
			return Task.FromResult(StartAsyncInvoked?.Invoke(args) ?? throw new Exception($"Call site is not subscribed to {nameof(StartAsyncInvoked)} event, which is mandatory for this mock to work."));
		}
	}
}