using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;

namespace TomiSoft.YouTubeDownloader.WebUI.HostedServices {
	public class CleanupHostedService : BackgroundService {
		private readonly IDownloadedFileCleanupService cleanupService;
		private readonly ILogger<BackgroundDownloaderService> logger;

		public CleanupHostedService(IDownloadedFileCleanupService cleanupService, ILogger<BackgroundDownloaderService> logger) {
			this.cleanupService = cleanupService;
			this.logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			logger.LogInformation($"{nameof(CleanupHostedService)} started.");
			while (!stoppingToken.IsCancellationRequested) {
				cleanupService.RunCleanup();
				await Task.Delay(1000, stoppingToken).ContinueWith(x => { });
			}

			logger.LogInformation($"{nameof(CleanupHostedService)} stopped due to cancellation request.");
		}
	}
}