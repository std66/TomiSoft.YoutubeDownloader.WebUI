using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
    public class YoutubeConfiguration : IYoutubeDlConfiguration, IDownloaderServiceConfiguration {
        public string ExecutablePath { get; set; }
        public int DeleteFilesAfterMinutesElapsed { get; set; }
        public string DownloadPath { get; set; }
    }
}
