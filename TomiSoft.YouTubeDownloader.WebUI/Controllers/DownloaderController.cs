using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.Data;
using TomiSoft.YouTubeDownloader.WebUI.HostedServices;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Controllers {
    public class DownloaderController : Controller
    {
        private readonly IMediaDownloader youtubeDl;
        private readonly IDownloaderService downloaderService;
        private readonly IFilenameDatabase filenameDatabase;

        public DownloaderController(IMediaDownloader youtubeDl, IDownloaderService downloaderService, IFilenameDatabase filenameDatabase) {
            this.youtubeDl = youtubeDl;
            this.downloaderService = downloaderService;
            this.filenameDatabase = filenameDatabase;
        }

        public IActionResult GetMediaInformation([FromQuery] Uri MediaUri) {
            IMediaInformation info = this.youtubeDl.GetMediaInformation(MediaUri);
            this.filenameDatabase.AddFilename(MediaUri, FilenameHelper.RemoveNotAllowedChars(info.Title));

            return new JsonResult(info);
        }

        public IActionResult DownloadFile([FromQuery] Guid DownloadId) {
            IDownload progress;

            try {
                progress = this.downloaderService.GetDownloadStatus(DownloadId);
            }
            catch (InvalidOperationException) {
                return new ErrorResponse(ErrorCodes.DownloadNotFound, HttpStatusCode.NotFound).AsJsonResult();
            }

            if (progress.Status != DownloadState.Completed) {
                return new ErrorResponse(ErrorCodes.DownloadNotCompleted, HttpStatusCode.PreconditionRequired).AsJsonResult();
            }

            Stream s = System.IO.File.OpenRead(progress.Filename);
            var contentType = "application/octet-stream";

            string fileName = this.filenameDatabase.GetFilename(DownloadId);
            if (fileName != null) {
                fileName += Path.GetExtension(progress.Filename);
            }
            else {
                fileName = progress.Filename;
            }

            return File(s, contentType, fileName);
        }
    }
}