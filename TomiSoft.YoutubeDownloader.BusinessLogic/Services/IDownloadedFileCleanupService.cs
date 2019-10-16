using TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
    public interface IDownloadedFileCleanupService {
        void MarkToCleanup(CompletedQueuedDownloadBM download);
        void RunCleanup();
    }
}