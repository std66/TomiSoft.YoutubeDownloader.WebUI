using TomiSoft.YoutubeDownloader;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
    public class YoutubeConfiguration : IYoutubeDlConfiguration, IDownloaderServiceConfiguration {
        public string ExecutablePath { get; set; }
        public int MaximumParallelDownloads { get; set; }
        public int DeleteFilesAfterMinutesElapsed { get; set; }
    }
}
