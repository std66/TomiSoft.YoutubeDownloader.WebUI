using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;

namespace TomiSoft.YouTubeDownloader.WebUI.Configuration {
    public class DataRetentionConfiguration : IDataRetentionConfiguration {
        public bool Enabled { get; set; }

        public int DeleteFilesAfterMinutesElapsed { get; set; }
    }
}
