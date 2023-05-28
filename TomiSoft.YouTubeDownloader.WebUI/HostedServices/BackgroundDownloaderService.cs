using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TomiSoft.Common.Hosting;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YouTubeDownloader.BusinessLogic.BusinessModels;

namespace TomiSoft.YouTubeDownloader.WebUI.HostedServices {
	public class BackgroundDownloaderService : BackgroundService {
		private readonly ILogger<BackgroundDownloaderService> logger;
		private readonly IDownloadManagementService downloadManager;
		private readonly IQueueService<DownloadRequestBM> requestQueue;
		private readonly IMaintenanceService maintenanceService;

		public BackgroundDownloaderService(ILogger<BackgroundDownloaderService> logger, IDownloadManagementService downloadManager, IQueueService<DownloadRequestBM> requestQueue, IMaintenanceService maintenanceService) {
			this.logger = logger;
			this.downloadManager = downloadManager;
			this.requestQueue = requestQueue;
			this.maintenanceService = maintenanceService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			this.logger.LogInformation($"{nameof(BackgroundDownloaderService)} has started.");

			while (!stoppingToken.IsCancellationRequested) {
				await WaitForMaintenanceAsync(stoppingToken);
				await StartNewDownloadsFromQueueAsync(stoppingToken);
			}

			this.logger.LogInformation($"{nameof(BackgroundDownloaderService)} has stopped because of a cancellation request.");
		}

		private async Task StartNewDownloadsFromQueueAsync(CancellationToken cancellationToken) {
			await this.requestQueue.WaitForAvailableItemAsync(cancellationToken);

			if (this.requestQueue.TryDequeue(out DownloadRequestBM request))
				downloadManager.StartDownload(request);
		}

		private Task WaitForMaintenanceAsync(CancellationToken stoppingToken) {
			if (maintenanceService.IsMaintenanceRunning) {
				return Task.Delay(500, stoppingToken);
			}

			return Task.CompletedTask;
		}
	}
}