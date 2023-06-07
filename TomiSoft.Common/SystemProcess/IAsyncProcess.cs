using System.Threading.Tasks;

namespace TomiSoft.Common.SystemProcess {
	public interface IAsyncProcess {
		Task<ProcessExecutionResult> StartAsync(params string[] args);
	}
}
