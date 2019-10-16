using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.BusinessLogic.Data;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
    public class MaintenanceService : IMaintenanceService {
        private readonly ILogger<MaintenanceService> logger;
        private readonly IMediaDownloader youtubeDl;
        private readonly IDownloadManagementService statusManager;
        private readonly IPersistedServiceStatusDataManager serviceStatus;

        public MaintenanceService(ILogger<MaintenanceService> logger, IMediaDownloader youtubeDl, IDownloadManagementService statusManager, IPersistedServiceStatusDataManager serviceStatus) {
            this.logger = logger;
            this.youtubeDl = youtubeDl;
            this.statusManager = statusManager;
            this.serviceStatus = serviceStatus;
        }

        public bool IsMaintenanceRunning { get; private set; }

        public async Task RunMaintenanceAsync() {
            await PrepareForMaintenanceAsync();

            if (this.IsMaintenanceRunning && statusManager.ActiveDownloadCount == 0) {
                UpdateMediaDownloader();
            }
        }

        private void UpdateMediaDownloader() {
            this.logger.LogInformation("Starting daily maintenance update...");

            string OldVersion = this.youtubeDl.GetVersion();
            this.youtubeDl.Update();
            string NewVersion = this.youtubeDl.GetVersion();

            if (OldVersion != NewVersion)
                this.logger.LogInformation($"Daily maintenance update has completed. Updated from version '{OldVersion}' to '{NewVersion}'.");
            else
                this.logger.LogInformation($"Daily maintenance update has completed. There is no new version available. Current version is '{NewVersion}'.");

            this.serviceStatus.LastUpdate = DateTime.UtcNow;
            this.IsMaintenanceRunning = false;
        }

        private async Task PrepareForMaintenanceAsync() {
            if (!this.IsMaintenanceRunning && DateTime.UtcNow - this.serviceStatus.LastUpdate > TimeSpan.FromDays(1)) {
                this.IsMaintenanceRunning = true;
                this.logger.LogInformation($"Preparing for daily maintenance update. There are {statusManager.ActiveDownloadCount} downloads to be completed before updating.");

                do {
                    await Task.Delay(1000);
                } while (statusManager.ActiveDownloadCount > 0);
            }
        }
    }
}
