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

        public IActionResult GetMediaInformation([FromQuery] string MediaUri) {
            if (String.IsNullOrWhiteSpace(MediaUri))
                return new ErrorResponse(ErrorCodes.MediaUriIsEmpty, HttpStatusCode.BadRequest).AsJsonResult();

            if (!Uri.IsWellFormedUriString(MediaUri, UriKind.Absolute))
                return new ErrorResponse(ErrorCodes.MalformedMediaUri, HttpStatusCode.BadRequest).AsJsonResult();

            Uri mediaUri = new Uri(MediaUri);

            IMediaInformation info = this.youtubeDl.GetMediaInformation(mediaUri);
            this.filenameDatabase.AddFilename(mediaUri, FilenameHelper.RemoveNotAllowedChars(info.Title));

            return new JsonResult(info);
        }

        public IActionResult EnqueueDownload([FromQuery] string MediaUri, [FromQuery] string MediaFormat) {
            MediaFormat TargetFormat = TomiSoft.YoutubeDownloader.Media.MediaFormat.Video;

            if (MediaFormat == "mp3audio") {
                TargetFormat = TomiSoft.YoutubeDownloader.Media.MediaFormat.MP3Audio;
            }

            Uri mediaUri = new Uri(MediaUri);
            Guid downloadId = downloaderService.EnqueueDownload(mediaUri, TargetFormat);
            this.filenameDatabase.AssignDownloadId(mediaUri, downloadId);

            return new JsonResult(new {
                DownloadId = downloadId
            });
        }

        public IActionResult GetProgress([FromQuery] string DownloadId) {
            if (Guid.TryParse(DownloadId, out Guid downloadGuid)) {
                try {
                    IDownload progress = this.downloaderService.GetDownloadStatus(downloadGuid);

                    return new JsonResult(new {
                        DownloadStatus = progress.Status.ToString(),
                        Percent = progress.Percentage
                    });
                }
                catch (InvalidOperationException) {
                    return new ErrorResponse(ErrorCodes.DownloadNotFound, HttpStatusCode.NotFound).AsJsonResult();
                }
            }
            else {
                return new ErrorResponse(ErrorCodes.InvalidDownloadId, HttpStatusCode.BadRequest).AsJsonResult();
            }
        }

        public IActionResult DownloadFile([FromQuery] string DownloadId) {
            if (Guid.TryParse(DownloadId, out Guid downloadGuid)) {
                try {
                    IDownload progress = this.downloaderService.GetDownloadStatus(downloadGuid);

                    if (progress.Status != DownloadState.Completed) {
                        return new ErrorResponse(ErrorCodes.DownloadNotCompleted, HttpStatusCode.PreconditionRequired).AsJsonResult();
                    }

                    Stream s = System.IO.File.OpenRead(progress.Filename);
                    var contentType = "application/octet-stream";

                    string fileName = this.filenameDatabase.GetFilename(downloadGuid);
                    if (fileName != null) {
                        fileName += Path.GetExtension(progress.Filename);
                    }
                    else {
                        fileName = progress.Filename;
                    }

                    return File(s, contentType, fileName);
                }
                catch (InvalidOperationException) {
                    return new ErrorResponse(ErrorCodes.DownloadNotFound, HttpStatusCode.NotFound).AsJsonResult();
                }
            }
            else {
                return new ErrorResponse(ErrorCodes.InvalidDownloadId, HttpStatusCode.BadRequest).AsJsonResult();
            }
        }
    }
}