using System;
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

        public IMediaInformation GetMediaInformation(Uri MediaUri) {
            return mediaDownloader.GetMediaInformation(MediaUri);
        }

        public string GetVersion() {
            return mediaDownloader.GetVersion();
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
