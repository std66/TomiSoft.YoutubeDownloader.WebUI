﻿using System;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.WebUI.Metrics {
	public class MediaDownloaderMetricsDecorator : IMediaDownloader {
		private readonly IMediaDownloader _downloader;
		private readonly IMediaDownloadMetrics metrics;

		public MediaDownloaderMetricsDecorator(IMediaDownloader downloader, IMediaDownloadMetrics metrics) {
			this.metrics = metrics;
			_downloader = downloader;
		}

		public Task<IMediaInformation> GetMediaInformationAsync(Uri MediaUri) {
			return _downloader.GetMediaInformationAsync(MediaUri);
		}

		public Task<string> GetVersionAsync() {
			return _downloader.GetVersionAsync();
		}

		public IDownload PrepareDownload(Uri MediaUri, MediaFormat MediaFormat, string downloadDirectory) {
			IDownload download = _downloader.PrepareDownload(MediaUri, MediaFormat, downloadDirectory);
			return new DownloadMetricsDecorator(download, metrics);
		}

		public void Update() {
			_downloader.Update();
		}
	}
}