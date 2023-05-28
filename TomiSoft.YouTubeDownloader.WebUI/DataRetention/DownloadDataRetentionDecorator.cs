using System;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YoutubeDownloader.Downloading;

namespace TomiSoft.YouTubeDownloader.WebUI.DataRetention {
	public class DownloadDataRetentionDecorator : IDownload {
		private readonly IDownload decoratedDownload;
		private readonly IDownloadedFileCleanupService cleanupService;

		public DownloadDataRetentionDecorator(IDownload decoratedDownload, IDownloadedFileCleanupService cleanupService) {
			this.decoratedDownload = decoratedDownload;
			this.cleanupService = cleanupService;

			this.decoratedDownload.DownloadStatusChanged += DecoratedDownload_DownloadStatusChanged;
		}

		private void DecoratedDownload_DownloadStatusChanged(object sender, DownloadState e) {
			if (e == DownloadState.Completed) {
				cleanupService.MarkToCleanup(this);
			}
		}

		public string Filename => decoratedDownload.Filename;

		public double Percentage => decoratedDownload.Percentage;

		public string ErrorMessage => decoratedDownload.ErrorMessage;

		public string CommandLine => decoratedDownload.CommandLine;

		public DownloadState Status => decoratedDownload.Status;

		public event EventHandler<DownloadState> DownloadStatusChanged {
			add {
				decoratedDownload.DownloadStatusChanged += value;
			}

			remove {
				decoratedDownload.DownloadStatusChanged -= value;
			}
		}

		public event EventHandler<double> PercentageChanged {
			add {
				decoratedDownload.PercentageChanged += value;
			}

			remove {
				decoratedDownload.PercentageChanged -= value;
			}
		}

		public void Dispose() {
			this.decoratedDownload.DownloadStatusChanged -= DecoratedDownload_DownloadStatusChanged;
			decoratedDownload.Dispose();
		}

		public void Start() {
			decoratedDownload.Start();
		}
	}
}