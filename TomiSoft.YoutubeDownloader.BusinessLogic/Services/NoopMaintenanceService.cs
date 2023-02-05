using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
    public class NoopMaintenanceService : IMaintenanceService {
        public bool IsMaintenanceRunning => false;

        public NoopMaintenanceService(ILogger<NoopMaintenanceService> logger) {
            logger.LogInformation("Built-in automatic updater is disabled.");
        }

        public Task RunMaintenanceAsync() {
            return Task.CompletedTask;
        }
    }
}
