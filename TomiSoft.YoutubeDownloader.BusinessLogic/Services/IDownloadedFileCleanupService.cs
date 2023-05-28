using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
	public interface IDownloadedFileCleanupService {
		void MarkToCleanup(IDownload download);
		void RunCleanup();
	}
}