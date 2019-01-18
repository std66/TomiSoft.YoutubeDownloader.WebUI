using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TomiSoft.Common.FileManagement;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YouTubeDownloader.WebUI.Core;

namespace TomiSoft.YouTubeDownloader.WebUI.HostedServices {
    public class BackgroundDownloaderService : BackgroundService, IDownloaderService {
        private const string LastUpdateFileName = "LastUpdate.DateTime.txt";

        private readonly IMediaDownloader YoutubeDl;
        private readonly IDownloaderServiceConfiguration ServiceConfiguration;
        private readonly ILogger logger;
        private readonly IFileManager fileManager;

        private readonly ConcurrentQueue<QueuedDownload> QueuedDownloads = new ConcurrentQueue<QueuedDownload>();
        private readonly List<QueuedDownload> RunningDownloads = new List<QueuedDownload>();
        private readonly List<QueuedDownload> CompletedDownloads = new List<QueuedDownload>();

        private DateTime LastUpdate {
            get {
                return this.mLastUpdate;
            }
            set {
                this.mLastUpdate = value;
                this.fileManager.TryCreateTextFile(LastUpdateFileName, value.ToString(), Common.FileManagement.Permissions.FileCreationPermission.AllowOverwrite, out _);
            }
        }

        private bool IsUpdating = false;
        private DateTime mLastUpdate = DateTime.UtcNow;

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
            this.logger.LogInformation($"{nameof(BackgroundDownloaderService)} has started.");
            LoadLastUpdateTimeFromFile();

            while (!stoppingToken.IsCancellationRequested) {
                StartNewDownloadsFromQueue();
                RemoveCompletedDownloads();
                UpdateDownloader();

                await Task.Delay(500);
            }

            this.logger.LogInformation($"{nameof(BackgroundDownloaderService)} has stopped because of a cancellation request.");
        }

        private void LoadLastUpdateTimeFromFile() {
            IFile file = this.fileManager.GetFile(LastUpdateFileName);
            if (file.Exists) {
                this.LastUpdate = DateTime.Parse(file.ReadAllText());
                this.logger.LogInformation($"Last maintenance update was executed on {this.LastUpdate}");
            }
            else {
                this.LastUpdate = DateTime.UtcNow.AddDays(-2);
                this.logger.LogInformation("There is no last maintenance update status persisted. Update process has enforced.");
            }
        }

        private void UpdateDownloader() {
            if (!this.IsUpdating && DateTime.UtcNow - this.LastUpdate > TimeSpan.FromDays(1)) {
                this.IsUpdating = true;
                this.logger.LogInformation($"Preparing for daily maintenance update. There are {this.RunningDownloads.Count} downloads to be completed before updating.");
            }

            if (this.IsUpdating && RunningDownloads.Count == 0) {
                this.logger.LogInformation("Starting daily maintenance update...");

                string OldVersion = this.YoutubeDl.GetVersion();
                this.YoutubeDl.Update();
                string NewVersion = this.YoutubeDl.GetVersion();

                if (OldVersion != NewVersion)
                    this.logger.LogInformation($"Daily maintenance update has completed. Updated from version '{OldVersion}' to '{NewVersion}'.");
                else
                    this.logger.LogInformation($"Daily maintenance update has completed. There is no new version available. Current version is '{NewVersion}'.");

                this.LastUpdate = DateTime.UtcNow;
                this.IsUpdating = false;
            }
        }

        private void StartNewDownloadsFromQueue() {
            if (this.IsUpdating)
                return;

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
