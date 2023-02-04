namespace TomiSoft.YoutubeDownloader.BusinessLogic.Configuration {
    public interface IDataRetentionConfiguration {
        bool Enabled { get; }
        int DeleteFilesAfterMinutesElapsed { get; }
    }
}
