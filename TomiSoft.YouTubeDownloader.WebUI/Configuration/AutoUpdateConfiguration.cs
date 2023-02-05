using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;

namespace TomiSoft.YouTubeDownloader.WebUI.Configuration {
    public class AutoUpdateConfiguration : IAutoUpdateConfiguration {
        public bool Enabled { get; set; }

        public int UpdateIntervalInHours { get; set; }
    }
}
