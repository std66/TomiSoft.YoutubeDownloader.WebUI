using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.Core.FileManagement;

namespace TomiSoft.YouTubeDownloader.WebUI.HostedServices {
    public class BackgroundDownloaderService : BackgroundService, IDownloaderService {
        private readonly IMediaDownloader YoutubeDl;
        private readonly IDownloaderServiceConfiguration ServiceConfiguration;
        private readonly ILogger logger;
        private readonly IFileManager fileManager;

        private readonly ConcurrentQueue<QueuedDownload> QueuedDownloads = new ConcurrentQueue<QueuedDownload>();
        private readonly List<QueuedDownload> RunningDownloads = new List<QueuedDownload>();
        private readonly List<QueuedDownload> CompletedDownloads = new List<QueuedDownload>();

        public BackgroundDownloaderService(IMediaDownloader YoutubeDl, IDownloaderServiceConfiguration serviceConfiguration, ILogger<BackgroundDownloaderService> logger, IFileManager fileManager) {
            this.YoutubeDl = YoutubeDl;
            this.ServiceConfiguration = serviceConfiguration;
            this.logger = logger;
            this.fileManager = fileManager;
        }

        public Guid EnqueueDownload(Uri MediaUri, MediaFormat mediaFormat) {
            QueuedDownload download = new QueuedDownload(
                DownloadId: Guid.NewGuid(),
                DownloadProgress: this.YoutubeDl.PrepareDownload(MediaUri, mediaFormat)
            );

            QueuedDownloads.Enqueue(download);

            return download.DownloadID;
        }

        public IDownload GetDownloadStatus(Guid DownloadID) {
            QueuedDownload result = this.QueuedDownloads.Concat(this.RunningDownloads).Concat(this.CompletedDownloads).FirstOrDefault(x => x.DownloadID == DownloadID);

            if (result == null)
                throw new InvalidOperationException($"Download not found with ID {DownloadID}.");

            return result.DownloadHandler;
        }

        private void DownloadCompleted(object sender, Guid DownloadID) {
            if (sender is QueuedDownload download) {
                RunningDownloads.Remove(download);
                CompletedDownloads.Add(download);

                this.logger.LogInformation($"Download completed with GUID: {download.DownloadID}");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                StartNewDownloadsFromQueue();
                RemoveCompletedDownloads();

                await Task.Delay(500);
            }
        }

        private void StartNewDownloadsFromQueue() {
            if (!QueuedDownloads.IsEmpty && RunningDownloads.Count < ServiceConfiguration.MaximumParallelDownloads) {
                if (QueuedDownloads.TryDequeue(out QueuedDownload download)) {
                    RunningDownloads.Add(download);
                    download.DownloadCompleted += this.DownloadCompleted;
                    download.DownloadHandler.Start();

                    this.logger.LogInformation($"Download started with GUID: {download.DownloadID}");
                }
            }
        }

        private void RemoveCompletedDownloads() {
            List<QueuedDownload> ToRemove = new List<QueuedDownload>();

            foreach (QueuedDownload download in this.CompletedDownloads.Where(x => DateTime.UtcNow - x.CompletedTimestamp > TimeSpan.FromMilliseconds(this.ServiceConfiguration.DeleteFilesAfterMillisecondsElapsed))) {
                if (download.DownloadHandler.Status == DownloadState.Completed) {
                    string filename = download.DownloadHandler.Filename;

                    IFile file = this.fileManager.GetFile(filename);
                    if (file.Exists && file.Delete()) {
                        this.logger.LogInformation($"File deleted for download with GUID: {download.DownloadID}");
                    }
                    else {
                        this.logger.LogWarning($"File cannot be deleted or not exists for download with GUID: {download.DownloadID}");
                    }
                }

                download.Dispose();
                this.logger.LogInformation($"Download instance is disposed with GUID: {download.DownloadID}");
                ToRemove.Add(download);
            }

            foreach (var item in ToRemove) {
                this.CompletedDownloads.Remove(item);
            }
        }
    }
}
