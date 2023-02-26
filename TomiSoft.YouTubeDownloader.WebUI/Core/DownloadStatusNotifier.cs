using Microsoft.AspNetCore.SignalR;
using System;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YouTubeDownloader.WebUI.Hubs;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
    public class DownloadStatusNotifier : IDownloadStatusNotifier {
        private readonly IHubContext<DownloadHub> hubContext;

        public DownloadStatusNotifier(IHubContext<DownloadHub> hubContext) {
            this.hubContext = hubContext;
        }

        public void Notify(Guid downloadId, DownloadState downloadState, double percentCompleted) { 
            hubContext
                .Clients
                .Group(downloadId.ToString())
                .SendAsync("UpdateDownloadStatus", new UpdateDownloadStatus(downloadState.ToString(), percentCompleted))
                .ConfigureAwait(false);
        }
    }
}
