using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using TomiSoft.Common.FileManagement;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YouTubeDownloader.BusinessLogic.DataManagement;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Controllers
{
    public class DownloaderController : Controller
    {
        private readonly IMediaDownloader youtubeDl;
        private readonly IDownloadManagementService downloaderService;
        private readonly IFilenameDatabase filenameDatabase;

        public DownloaderController(IMediaDownloader youtubeDl, IDownloadManagementService downloaderService, IFilenameDatabase filenameDatabase) {
            this.youtubeDl = youtubeDl;
            this.downloaderService = downloaderService;
            this.filenameDatabase = filenameDatabase;
        }

        public IActionResult GetMediaInformation([FromQuery] Uri MediaUri) {
            IMediaInformation info = this.youtubeDl.GetMediaInformation(MediaUri);

            if (info.IsLiveStream)
            {
                return new ErrorResponse(ErrorCodes.LiveStreamsAreNotSupported, HttpStatusCode.Conflict).AsJsonResult();
            }

            this.filenameDatabase.AddFilename(MediaUri, FilenameHelper.RemoveNotAllowedChars(info.Title));

            return new JsonResult(info);
        }

        public IActionResult DownloadFile([FromQuery] Guid DownloadId) {
            DownloadStatusBM progress;

            try {
                progress = this.downloaderService.GetDownloadStatus(DownloadId);
            }
            catch (InvalidOperationException) {
                return new ErrorResponse(ErrorCodes.DownloadNotFound, HttpStatusCode.NotFound).AsJsonResult();
            }

            if (progress.Status != DownloadState.Completed) {
                return new ErrorResponse(ErrorCodes.DownloadNotCompleted, HttpStatusCode.PreconditionRequired).AsJsonResult();
            }

            IFile file = this.downloaderService.GetDownloadedFile(DownloadId);

            Stream s = file.Open(FileMode.Open);
            var contentType = "application/octet-stream";

            string fileName = this.filenameDatabase.GetFilename(DownloadId);
            if (fileName != null) {
                fileName += Path.GetExtension(file.Path);
            }
            else {
                fileName = file.Path;
            }

            return File(s, contentType, fileName);
        }
    }
}