using System;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
    public interface IDownloadStatusNotifier {
        void Notify(Guid downloadId, DownloadState downloadState, double percentCompleted);
    }
}