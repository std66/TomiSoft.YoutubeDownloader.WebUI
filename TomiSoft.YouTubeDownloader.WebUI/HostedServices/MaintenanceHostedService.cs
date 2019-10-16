using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;

namespace TomiSoft.YouTubeDownloader.WebUI.HostedServices {
    public class MaintenanceHostedService : BackgroundService {
        private readonly ILogger<MaintenanceHostedService> logger;
        private readonly IMaintenanceService maintenanceService;

        public MaintenanceHostedService(ILogger<MaintenanceHostedService> logger, IMaintenanceService maintenanceService) {
            this.logger = logger;
            this.maintenanceService = maintenanceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            logger.LogInformation($"{nameof(MaintenanceHostedService)} started.");
            while (!stoppingToken.IsCancellationRequested) {
                await maintenanceService.RunMaintenanceAsync();
                await Task.Delay(60 * 1000, stoppingToken).ContinueWith(x => { });
            }

            logger.LogInformation($"{nameof(MaintenanceHostedService)} stopped due to cancellation request.");
        }
    }
}
