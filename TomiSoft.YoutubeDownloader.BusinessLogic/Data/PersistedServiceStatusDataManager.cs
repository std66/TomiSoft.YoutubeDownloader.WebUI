using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.BusinessLogic.Dependencies.IO;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Data {
	public class PersistedServiceStatusDataManager : IPersistedServiceStatusDataManager {
		private const string LastUpdateFileName = "LastUpdate.DateTime.txt";
		private readonly IFileManager fileManager;
		private readonly ILogger<PersistedServiceStatusDataManager> logger;

		private DateTime? lastUpdate;

		public PersistedServiceStatusDataManager(IFileManager fileManager, ILogger<PersistedServiceStatusDataManager> logger) {
			this.fileManager = fileManager;
			this.logger = logger;
		}

		public async Task<DateTime?> GetLastUpdateTimeAsync() {
			if (lastUpdate.HasValue)
				return lastUpdate.Value;

			if (this.fileManager.TryGetFile(LastUpdateFileName, out IFile file)) {
				lastUpdate = DateTime.Parse(await file.ReadContentsAsStringAsync());
				this.logger.LogInformation($"Last maintenance update was executed on {lastUpdate}");

				return lastUpdate.Value;
			}

			this.logger.LogInformation("There is no last maintenance update status persisted. Update process has enforced.");
			return null;
		}

		public async Task SaveLastUpdateTimeAsync(DateTime lastUpdateTime) {
			lastUpdate = lastUpdateTime;
			if (!(await this.fileManager.TryCreateFileAsync(LastUpdateFileName, lastUpdateTime.ToString(), true)).Successful)
				logger.LogWarning($"Failed to write file: {LastUpdateFileName}");
		}
	}
}