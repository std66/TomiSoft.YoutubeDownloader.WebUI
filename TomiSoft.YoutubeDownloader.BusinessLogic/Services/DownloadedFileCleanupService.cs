using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TomiSoft.Common.FileManagement;
using TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
    public class DownloadedFileCleanupService : IDownloadedFileCleanupService {
        private readonly List<CompletedQueuedDownloadBM> completedDownloads = new List<CompletedQueuedDownloadBM>();
        private readonly ILogger<DownloadedFileCleanupService> logger;
        private readonly IFileManager fileManager;
        private readonly IDownloaderServiceConfiguration serviceConfiguration;

        public DownloadedFileCleanupService(ILogger<DownloadedFileCleanupService> logger, IFileManager fileManager, IDownloaderServiceConfiguration serviceConfiguration) {
            this.logger = logger;
            this.fileManager = fileManager;
            this.serviceConfiguration = serviceConfiguration;
        }

        public void MarkToCleanup(CompletedQueuedDownloadBM download) {
            this.completedDownloads.Add(download);
        }

        public void RunCleanup() {
            List<CompletedQueuedDownloadBM> ToRemove = new List<CompletedQueuedDownloadBM>();

            foreach (CompletedQueuedDownloadBM download in this.completedDownloads.Where(IsDeletable)) {
                if (download.DownloadHandler.Status == DownloadState.Completed) {
                    string filename = download.DownloadHandler.Filename;

                    IFile file = this.fileManager.GetFile(filename);
                    if (file.Exists && file.Delete()) {
                        this.logger.LogInformation($"File deleted for download with GUID: {download.DownloadID}");
                        ToRemove.Add(download);
                    }
                    else {
                        this.logger.LogWarning($"File cannot be deleted or not exists for download with GUID: {download.DownloadID}");
                    }
                }
            }

            foreach (var item in ToRemove) {
                this.completedDownloads.Remove(item);
            }
        }

        private bool IsDeletable(CompletedQueuedDownloadBM x) {
            return DateTime.UtcNow - x.CompletionTimestampUtc > TimeSpan.FromMinutes(this.serviceConfiguration.DeleteFilesAfterMinutesElapsed);
        }
    }
}
