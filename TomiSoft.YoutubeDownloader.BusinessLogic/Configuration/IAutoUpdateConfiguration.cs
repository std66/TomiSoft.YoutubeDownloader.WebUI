namespace TomiSoft.YoutubeDownloader.BusinessLogic.Configuration {
	public interface IAutoUpdateConfiguration {
		bool Enabled { get; }
		int UpdateIntervalInHours { get; }
	}
}