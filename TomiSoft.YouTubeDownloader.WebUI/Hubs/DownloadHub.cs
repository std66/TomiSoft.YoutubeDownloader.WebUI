using Microsoft.AspNetCore.SignalR;
using System;
using System.Net;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.Data;
using TomiSoft.YouTubeDownloader.WebUI.HostedServices;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Hubs {
    public class DownloadHub : Hub {
        private readonly IDownloaderService downloaderService;
        private readonly IFilenameDatabase filenameDatabase;

        public DownloadHub(IDownloaderService downloaderService, IFilenameDatabase filenameDatabase) {
            this.downloaderService = downloaderService;
            this.filenameDatabase = filenameDatabase;
        }

        public async Task EnqueueDownload(string MediaUri, string MediaFormat) {
            MediaFormat TargetFormat = YoutubeDownloader.Media.MediaFormat.Video;

            if (MediaFormat == "mp3audio") {
                TargetFormat = YoutubeDownloader.Media.MediaFormat.MP3Audio;
            }

            Uri mediaUri = new Uri(MediaUri);
            Guid downloadId = downloaderService.EnqueueDownload(mediaUri, TargetFormat);
            this.filenameDatabase.AssignDownloadId(mediaUri, downloadId);

            await Groups.AddToGroupAsync(Context.ConnectionId, downloadId.ToString());
            await Clients.Caller.SendAsync("UseDownloadId", downloadId);
        }

        public async Task RequestProgress(string DownloadId) {
            if (Guid.TryParse(DownloadId, out Guid downloadGuid)) {
                IDownload progress = this.downloaderService.GetDownloadStatus(downloadGuid);

                await Clients.Caller.SendAsync("UpdateDownloadStatus", new {
                    DownloadStatus = progress.Status.ToString(),
                    Percent = progress.Percentage
                });
            }
        }
    }
}
