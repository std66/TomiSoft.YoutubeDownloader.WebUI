using System;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.WebUI.DataRetention {
	public class MediaDownloaderDataRetentionDecorator : IMediaDownloader {
		private readonly IMediaDownloader mediaDownloader;
		private readonly IDownloadedFileCleanupService cleanupService;

		public MediaDownloaderDataRetentionDecorator(IMediaDownloader mediaDownloader, IDownloadedFileCleanupService cleanupService) {
			this.mediaDownloader = mediaDownloader;
			this.cleanupService = cleanupService;
		}

		public Task<IMediaInformation> GetMediaInformationAsync(Uri MediaUri) {
			return mediaDownloader.GetMediaInformationAsync(MediaUri);
		}

		public Task<string> GetVersionAsync() {
			return mediaDownloader.GetVersionAsync();
		}

		public IDownload PrepareDownload(Uri MediaUri, MediaFormat MediaFormat, string downloadDirectory) {
			return new DownloadDataRetentionDecorator(
				decoratedDownload: mediaDownloader.PrepareDownload(MediaUri, MediaFormat, downloadDirectory),
				cleanupService: cleanupService
			);
		}

		public void Update() {
			mediaDownloader.Update();
		}
	}
}