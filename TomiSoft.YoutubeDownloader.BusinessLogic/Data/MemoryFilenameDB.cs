using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace TomiSoft.YouTubeDownloader.BusinessLogic.DataManagement {
	public class MemoryFilenameDB : IFilenameDatabase {
		private class FilenameRecord {
			public DateTime CreateTime { get; set; }
			public Uri MediaUri { get; set; }
			public Guid? DownloadId { get; set; }
			public string Filename { get; set; }
		}

		private const int RunCleanUpIntervalInMinutes = 1;
		private const int CleanUpNotAssignedRecordsAfterMinutes = 1;
		private const int CleanUpAssignedRecordsAfterMinutes = 60;

		private readonly List<FilenameRecord> records = new List<FilenameRecord>();
		private readonly Timer cleanupTimer;

		public MemoryFilenameDB() {
			this.cleanupTimer = new Timer(RunCleanUpIntervalInMinutes * 60 * 1000) {
				Enabled = false
			};

			this.cleanupTimer.Elapsed += CleanUp;

			this.cleanupTimer.Start();
		}

		private void CleanUp(object sender, ElapsedEventArgs e) {
			this.records.RemoveAll(x => DateTime.UtcNow - x.CreateTime > TimeSpan.FromMinutes(CleanUpNotAssignedRecordsAfterMinutes) && !x.DownloadId.HasValue);
			this.records.RemoveAll(x => DateTime.UtcNow - x.CreateTime > TimeSpan.FromMinutes(CleanUpAssignedRecordsAfterMinutes) && x.DownloadId.HasValue);
		}

		public bool AddFilename(Uri MediaUri, string Filename) {
			FilenameRecord newRecord = new FilenameRecord() {
				CreateTime = DateTime.UtcNow,
				MediaUri = MediaUri,
				Filename = Filename,
				DownloadId = null
			};

			this.records.Add(newRecord);

			return true;
		}

		public bool AssignDownloadId(Uri MediaUri, Guid DownloadId) {
			FilenameRecord record = this.records.FirstOrDefault(x => x.MediaUri == MediaUri && !x.DownloadId.HasValue);
			if (record != null)
				record.DownloadId = DownloadId;

			return record != null;
		}

		public void DeleteFilename(Guid DownloadId) {
			records.RemoveAll(x => x.DownloadId.HasValue && x.DownloadId == DownloadId);
		}

		public string GetFilename(Guid DownloadId) {
			FilenameRecord record = this.records.FirstOrDefault(x => x.DownloadId.HasValue && x.DownloadId == DownloadId);
			return record?.Filename;
		}

		public void Dispose() {
			this.cleanupTimer.Stop();
			this.cleanupTimer.Elapsed -= CleanUp;
		}
	}
}