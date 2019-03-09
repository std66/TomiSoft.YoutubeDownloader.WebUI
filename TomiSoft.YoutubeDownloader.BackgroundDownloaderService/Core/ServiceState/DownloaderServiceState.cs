using Microsoft.Extensions.Logging;
using System;
using TomiSoft.Common.FileManagement;
using TomiSoft.Common.FileManagement.Permissions;
using TomiSoft.Common.SystemClock;

namespace TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core.ServiceState {
    public class DownloaderServiceState : IDownloaderServiceState {
        private readonly string LastUpdateFileName = "LastUpdate.DateTime.txt";
        private readonly IFileManager FileManager;
        private readonly ISystemClock SystemClock;
        private readonly ILogger logger;

        private DateTime mLastUpdate;

        public DateTime LastUpdateTime {
            get {
                return this.mLastUpdate;
            }

            set {
                this.mLastUpdate = value;
                this.Save();
            }
        }

        public DownloaderServiceState(IFileManager fileManager, ISystemClock systemClock, ILogger<DownloaderServiceState> logger) {
            this.FileManager = fileManager;
            this.SystemClock = systemClock;
            this.logger = logger;

            IFile file = fileManager.GetFile(this.LastUpdateFileName);
            if (file.Exists) {
                this.mLastUpdate = DateTime.Parse(file.ReadAllText());
                this.logger.LogInformation($"Last maintenance update was executed on {this.mLastUpdate}");
            }
            else {
                this.mLastUpdate = systemClock.UtcNow.AddDays(-2);
                this.logger.LogInformation("There is no last maintenance update status persisted. Update process has enforced.");
            }
        }

        private void Save() {
            if (!this.FileManager.TryCreateTextFile(LastUpdateFileName, this.mLastUpdate.ToString(), FileCreationPermission.AllowOverwrite, out _)) {
                this.logger.LogWarning("Failed to save service state to disk.");
            }
        }
    }
}
