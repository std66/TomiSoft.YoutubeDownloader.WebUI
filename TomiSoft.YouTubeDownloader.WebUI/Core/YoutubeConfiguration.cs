using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;

namespace TomiSoft.YouTubeDownloader.WebUI.Core
{
    public class YoutubeConfiguration : IYoutubeDlConfiguration, IDownloaderServiceConfiguration {
        public string ExecutablePath { get; set; }
        public int MaximumParallelDownloads { get; set; }
        public int DeleteFilesAfterMillisecondsElapsed { get; set; }
        public string DownloadPath { get; set; }
    }
}
