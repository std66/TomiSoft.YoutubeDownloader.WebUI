using System.Collections.Generic;
using TomiSoft.Common.SystemProcess;

namespace TomiSoft.YoutubeDownloader {
	public interface IProcessFactory {
		IProcess Create(params string[] args);
		IProcess Create(IEnumerable<string> args);
		IAsyncProcess CreateAsyncProcess();
	}
}