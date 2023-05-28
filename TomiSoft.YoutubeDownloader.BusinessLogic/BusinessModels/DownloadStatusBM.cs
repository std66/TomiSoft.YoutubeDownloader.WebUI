using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels {
	public class DownloadStatusBM {
		public DownloadStatusBM(double percentage, DownloadState status) {
			Percentage = percentage;
			Status = status;
		}

		public double Percentage { get; }
		public DownloadState Status { get; }
	}
}