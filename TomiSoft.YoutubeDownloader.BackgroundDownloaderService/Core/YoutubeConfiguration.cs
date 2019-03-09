using TomiSoft.YoutubeDownloader;

namespace TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core {
    public class YoutubeConfiguration : IYoutubeDlConfiguration, IDownloaderServiceConfiguration {
        public string ExecutablePath { get; set; }
        public int MaximumParallelDownloads { get; set; }
        public int DeleteFilesAfterMillisecondsElapsed { get; set; }
    }
}
