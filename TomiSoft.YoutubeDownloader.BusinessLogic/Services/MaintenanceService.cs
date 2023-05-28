using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;
using TomiSoft.YoutubeDownloader.BusinessLogic.Data;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
	public class MaintenanceService : IMaintenanceService {
		private readonly ILogger<MaintenanceService> logger;
		private readonly IMediaDownloader youtubeDl;
		private readonly IDownloadManagementService statusManager;
		private readonly IPersistedServiceStatusDataManager serviceStatus;
		private readonly IAutoUpdateConfiguration autoUpdateConfiguration;
		private readonly IMaintenanceStatusNotifier maintenanceStatusNotifier;

		public MaintenanceService(ILogger<MaintenanceService> logger, IMediaDownloader youtubeDl, IDownloadManagementService statusManager, IPersistedServiceStatusDataManager serviceStatus, IAutoUpdateConfiguration autoUpdateConfiguration, IMaintenanceStatusNotifier maintenanceStatusNotifier) {
			this.logger = logger;
			this.youtubeDl = youtubeDl;
			this.statusManager = statusManager;
			this.serviceStatus = serviceStatus;
			this.autoUpdateConfiguration = autoUpdateConfiguration;
			this.maintenanceStatusNotifier = maintenanceStatusNotifier;
			logger.LogInformation($"Built-in automatic updates are enabled and will occur every {autoUpdateConfiguration.UpdateIntervalInHours} hours.");
		}

		public bool IsMaintenanceRunning { get; private set; }

		public async Task RunMaintenanceAsync() {
			await PrepareForMaintenanceAsync();

			if (this.IsMaintenanceRunning && statusManager.ActiveDownloadCount == 0) {
				UpdateMediaDownloader();
			}
		}

		private void UpdateMediaDownloader() {
			this.logger.LogInformation("Starting maintenance update...");
			maintenanceStatusNotifier.NotifyMaintenanceStart();

			string OldVersion = this.youtubeDl.GetVersion();
			this.youtubeDl.Update();
			string NewVersion = this.youtubeDl.GetVersion();

			string logNextUpdateTime = $"Next update is scheduled at {(DateTime.UtcNow + TimeSpan.FromHours(autoUpdateConfiguration.UpdateIntervalInHours)):o}";

			if (OldVersion != NewVersion)
				this.logger.LogInformation($"Maintenance update has completed. Updated from version '{OldVersion}' to '{NewVersion}'. {logNextUpdateTime}");
			else
				this.logger.LogInformation($"Maintenance update has completed. There is no new version available. Current version is '{NewVersion}'. {logNextUpdateTime}");

			this.serviceStatus.SaveLastUpdateTimeAsync(DateTime.UtcNow);
			this.IsMaintenanceRunning = false;
			maintenanceStatusNotifier.NotifyMaintenanceComplete();
		}

		private async Task PrepareForMaintenanceAsync() {
			if (IsMaintenanceRunning)
				return;

			if (DateTime.UtcNow - (await this.serviceStatus.GetLastUpdateTimeAsync()) < TimeSpan.FromHours(autoUpdateConfiguration.UpdateIntervalInHours))
				return;

			this.IsMaintenanceRunning = true;
			this.logger.LogInformation($"Preparing for maintenance update. There are {statusManager.ActiveDownloadCount} downloads to be completed before updating.");

			do {
				await Task.Delay(1000);
			} while (statusManager.ActiveDownloadCount > 0);
		}
	}
}