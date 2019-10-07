using System;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
    public interface IDownloadStatusNotifier {
        void Notify(Guid downloadId, DownloadState downloadState, double percentCompleted);
    }
}