using System;

namespace TomiSoft.YoutubeDownloader.Downloading {
	public interface IDownload : IDisposable {
		string Filename { get; }
		double Percentage { get; }
		string ErrorMessage { get; }
		string CommandLine { get; }
		DownloadState Status { get; }

		event EventHandler<DownloadState> DownloadStatusChanged;
		event EventHandler<double> PercentageChanged;

		void Start();
	}
}