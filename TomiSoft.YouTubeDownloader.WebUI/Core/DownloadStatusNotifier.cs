using Microsoft.AspNetCore.SignalR;
using System;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YouTubeDownloader.WebUI.Hubs;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
    public class DownloadStatusNotifier : IDownloadStatusNotifier {
        private readonly IHubContext<DownloadHub> hubContext;

        public DownloadStatusNotifier(IHubContext<DownloadHub> hubContext) {
            this.hubContext = hubContext;
        }

        public void Notify(Guid downloadId, DownloadState downloadState, double percentCompleted) { 
            hubContext.Clients.Group(downloadId.ToString()).SendAsync("UpdateDownloadStatus", new {
                DownloadStatus = downloadState.ToString(),
                Percent = percentCompleted
            }).ConfigureAwait(false);
        }
    }
}
