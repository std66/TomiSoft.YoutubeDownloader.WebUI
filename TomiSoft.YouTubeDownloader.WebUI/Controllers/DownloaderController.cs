using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Tomisoft.YoutubeDownloader;
using Tomisoft.YoutubeDownloader.Downloading;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.HostedServices;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Controllers {
    public class DownloaderController : Controller
    {
        private readonly IMediaDownloader youtubeDl;
        private readonly IDownloaderService downloaderService;

        public DownloaderController(IMediaDownloader youtubeDl, IDownloaderService downloaderService) {
            this.youtubeDl = youtubeDl;
            this.downloaderService = downloaderService;
        }

        public IActionResult GetMediaInformation([FromQuery] string MediaUri) {
            if (String.IsNullOrWhiteSpace(MediaUri))
                return new ErrorResponse(ErrorCodes.MediaUriIsEmpty, HttpStatusCode.BadRequest).AsJsonResult();

            if (!Uri.IsWellFormedUriString(MediaUri, UriKind.Absolute))
                return new ErrorResponse(ErrorCodes.MalformedMediaUri, HttpStatusCode.BadRequest).AsJsonResult();
            
            return new JsonResult(this.youtubeDl.GetMediaInformation(new Uri(MediaUri)));
        }

        public IActionResult EnqueueDownload([FromQuery] string MediaUri, [FromQuery] string MediaFormat) {
            Tomisoft.YoutubeDownloader.Media.MediaFormat TargetFormat = Tomisoft.YoutubeDownloader.Media.MediaFormat.Video;

            if (MediaFormat == "mp3audio") {
                TargetFormat = Tomisoft.YoutubeDownloader.Media.MediaFormat.MP3Audio;
            }
            
            Guid downloadId = downloaderService.EnqueueDownload(new Uri(MediaUri), TargetFormat);

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
                    var fileName = Path.GetFileName(progress.Filename);
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