using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TomiSoft.Common.FileManagement;
using TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YouTubeDownloader.BusinessLogic.BusinessModels;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services
{
    public class DownloadManagementService : IDownloadManagementService {
        private readonly List<QueuedDownloadBM> runningDownloads = new List<QueuedDownloadBM>();
        private readonly List<CompletedQueuedDownloadBM> completedDownloads = new List<CompletedQueuedDownloadBM>();

        private readonly ILogger<DownloadManagementService> logger;
        private readonly IDownloadStatusNotifier notifier;
        private readonly IMediaDownloader youtubeDl;
        private readonly IDownloaderServiceConfiguration serviceConfig;
        private readonly IFileManager fileManager;
        private readonly IDownloadedFileCleanupService cleanupService;

        public DownloadManagementService(ILogger<DownloadManagementService> logger, IDownloadStatusNotifier notifier, IMediaDownloader youtubeDl, IDownloaderServiceConfiguration serviceConfig, IFileManager fileManager, IDownloadedFileCleanupService cleanupService) {
            this.logger = logger;
            this.notifier = notifier;
            this.youtubeDl = youtubeDl;
            this.serviceConfig = serviceConfig;
            this.fileManager = fileManager;
            this.cleanupService = cleanupService;
        }

        public int ActiveDownloadCount => runningDownloads.Count;

        private void EventHandler_DownloadCompleted(QueuedDownloadBM download) {
            CompletedQueuedDownloadBM completedDownload = new CompletedQueuedDownloadBM(download.DownloadID, download.DownloadHandler, DateTime.UtcNow);

            runningDownloads.Remove(download);
            completedDownloads.Add(completedDownload);
            cleanupService.MarkToCleanup(completedDownload);

            this.logger.LogInformation($"Download completed with GUID: {download.DownloadID}");
        }

        public void StartDownload(DownloadRequestBM request) {
            QueuedDownloadBM download = new QueuedDownloadBM(
                downloadId: request.DownloadId,
                downloadProgress: this.youtubeDl.PrepareDownload(request.MediaUri, request.Format, serviceConfig.DownloadPath)
            );

            runningDownloads.Add(download);
            download.DownloadHandler.PercentageChanged += (o, e) => this.notifier.Notify(download.DownloadID, download.DownloadHandler.Status, e);
            download.DownloadHandler.DownloadStatusChanged += (o, e) => {
                this.notifier.Notify(download.DownloadID, e, download.DownloadHandler.Percentage);

                if (e == DownloadState.Completed)
                    EventHandler_DownloadCompleted(download);
                else if (e == DownloadState.Failed) {
                    string errorOutput = download.DownloadHandler.ErrorMessage.Replace("\r", "\\r").Replace("\n", "\\n");
                    logger.LogError($"Error occurred during download: CommandLine='{download.DownloadHandler.CommandLine}' StdErr='{errorOutput}'");
                }
            };
            download.DownloadHandler.Start();

            this.logger.LogInformation($"Download started with GUID: {download.DownloadID}");
        }

        public DownloadStatusBM GetDownloadStatus(Guid downloadID) {
            QueuedDownloadBM result = this.runningDownloads.Concat(this.completedDownloads).FirstOrDefault(x => x.DownloadID == downloadID);

            if (result == null)
                throw new InvalidOperationException($"Download not found with ID {downloadID}.");

            return new DownloadStatusBM(result.DownloadHandler.Percentage, result.DownloadHandler.Status);
        }

        public IFile GetDownloadedFile(Guid downloadId) {
            CompletedQueuedDownloadBM completedDownload = this.completedDownloads.First(x => x.DownloadID == downloadId);
            IFile file = fileManager.GetFile(completedDownload.DownloadHandler.Filename);

            this.completedDownloads.Remove(completedDownload);

            return file;
        }
    }
}
