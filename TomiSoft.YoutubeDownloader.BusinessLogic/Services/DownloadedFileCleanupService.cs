using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TomiSoft.Common.FileManagement;
using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
    public class DownloadedFileCleanupService : IDownloadedFileCleanupService {
        private class DownloadToDelete {
            public IDownload Download { get; set; }
            public DateTimeOffset CompletionTime { get; set; }
        }

        private readonly List<DownloadToDelete> completedDownloads = new List<DownloadToDelete>();
        private readonly ILogger<DownloadedFileCleanupService> logger;
        private readonly IFileManager fileManager;
        private readonly IDataRetentionConfiguration serviceConfiguration;

        public DownloadedFileCleanupService(ILogger<DownloadedFileCleanupService> logger, IFileManager fileManager, IDataRetentionConfiguration serviceConfiguration) {
            this.logger = logger;
            this.fileManager = fileManager;
            this.serviceConfiguration = serviceConfiguration;
        }

        public void MarkToCleanup(IDownload download) {
            this.completedDownloads.Add(
                new DownloadToDelete() {
                    Download = download,
                    CompletionTime = DateTimeOffset.UtcNow
                }
            );

            this.logger.LogInformation($"File is marked for deletion: {download.Filename}");
        }

        public void RunCleanup() {
            List<DownloadToDelete> ToRemove = new List<DownloadToDelete>();

            foreach (DownloadToDelete download in this.completedDownloads.Where(IsDeletable)) {
                string filename = download.Download.Filename;

                IFile file = this.fileManager.GetFile(filename);
                if (file.Exists && file.Delete()) {
                    this.logger.LogInformation($"File deleted: {file.Path}");
                    ToRemove.Add(download);
                }
                else {
                    this.logger.LogWarning($"File cannot be deleted or does not exists: {file.Path}");
                }
            }

            foreach (var item in ToRemove) {
                this.completedDownloads.Remove(item);
            }
        }

        private bool IsDeletable(DownloadToDelete x) {
            return 
                x.Download.Status == DownloadState.Completed &&
                DateTimeOffset.UtcNow - x.CompletionTime > TimeSpan.FromMinutes(this.serviceConfiguration.DeleteFilesAfterMinutesElapsed);
        }
    }
}
