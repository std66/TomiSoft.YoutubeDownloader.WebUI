using TomiSoft.YouTubeDownloader.WebUI.Core;

namespace TomiSoft.YoutubeDownloader.WebUI.Tests.Mocks {
    class DownloaderServiceConfiguration : IDownloaderServiceConfiguration {
        public int MaximumParallelDownloads => 3;

        public int DeleteFilesAfterMillisecondsElapsed => 10;
    }
}
