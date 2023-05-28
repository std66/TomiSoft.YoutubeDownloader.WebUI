using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;

namespace TomiSoft.YoutubeDownloader.WebUI.Tests.Mocks {
	class DownloaderServiceConfiguration : IDownloaderServiceConfiguration {
		public int MaximumParallelDownloads => 3;

		public int DeleteFilesAfterMinutesElapsed => 0;

		public string DownloadPath => "fakepath";
	}
}