using Microsoft.Extensions.Logging;
using System;
using TomiSoft.Common.FileManagement;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Data {
    public class PersistedServiceStatusDataManager : IPersistedServiceStatusDataManager {
        private const string LastUpdateFileName = "LastUpdate.DateTime.txt";
        private readonly IFileManager fileManager;
        private readonly ILogger<PersistedServiceStatusDataManager> logger;

        private DateTime lastUpdate;

        public PersistedServiceStatusDataManager(IFileManager fileManager, ILogger<PersistedServiceStatusDataManager> logger) {
            this.fileManager = fileManager;
            this.logger = logger;

            LoadFromFile();
        }

        public DateTime LastUpdate {
            get {
                return lastUpdate;
            }
            set {
                this.lastUpdate = value;
                this.fileManager.TryCreateTextFile(LastUpdateFileName, value.ToString(), Common.FileManagement.Permissions.FileCreationPermission.AllowOverwrite, out _);
            }
        }

        private void LoadFromFile() {
            IFile file = this.fileManager.GetFile(LastUpdateFileName);
            if (file.Exists) {
                this.lastUpdate = DateTime.Parse(file.ReadAllText());
                this.logger.LogInformation($"Last maintenance update was executed on {this.LastUpdate}");
            }
            else {
                this.lastUpdate = DateTime.UtcNow.AddDays(-2);
                this.logger.LogInformation("There is no last maintenance update status persisted. Update process has enforced.");
            }
        }

    }
}
