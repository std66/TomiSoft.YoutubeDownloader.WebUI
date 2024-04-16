using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YoutubeDownloader.BusinessLogic.Dependencies.IO;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YoutubeDownloader.Downloading;
using TomiSoft.YoutubeDownloader.Exceptions;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YouTubeDownloader.BusinessLogic.DataManagement;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Controllers {
	public class DownloaderController : Controller {
		private readonly IMediaDownloader youtubeDl;
		private readonly IDownloadManagementService downloaderService;
		private readonly IFilenameDatabase filenameDatabase;

		public DownloaderController(IMediaDownloader youtubeDl, IDownloadManagementService downloaderService, IFilenameDatabase filenameDatabase) {
			this.youtubeDl = youtubeDl;
			this.downloaderService = downloaderService;
			this.filenameDatabase = filenameDatabase;
		}

		public async Task<IActionResult> GetMediaInformationAsync([FromQuery] Uri MediaUri) {
			IMediaInformation info;

			try {
				info = await this.youtubeDl.GetMediaInformationAsync(MediaUri);
			}
			catch (PrivateMediaException) {
				return new ErrorResponse(ErrorCodes.PrivateMediaIsNotSupported, HttpStatusCode.Conflict).AsJsonResult();
			}
			catch (RestrictedBySubscriptionPolicyException) {
				return new ErrorResponse(ErrorCodes.RestrictedContentBySubscriptionPolicy, HttpStatusCode.Conflict).AsJsonResult();
			}

			if (info.IsLiveStream)
				return new ErrorResponse(ErrorCodes.LiveStreamsAreNotSupported, HttpStatusCode.Conflict).AsJsonResult();

			this.filenameDatabase.AddFilename(MediaUri, FilenameHelper.RemoveNotAllowedChars(info.Title));

			return new JsonResult(
				GetMediaInformationResponse.CreateFrom(info)
			);
		}

		public IActionResult DownloadFile([FromQuery] Guid DownloadId) {
			if (!this.downloaderService.TryGetDownloadStatus(DownloadId, out DownloadStatusBM progress))
				return new ErrorResponse(ErrorCodes.DownloadNotFound, HttpStatusCode.NotFound).AsJsonResult();

			if (progress.Status != DownloadState.Completed)
				return new ErrorResponse(ErrorCodes.DownloadNotCompleted, HttpStatusCode.PreconditionRequired).AsJsonResult();

			if (!this.downloaderService.TryGetDownloadedFile(DownloadId, out IFile file))
				return new ErrorResponse(ErrorCodes.DownloadNotFound, HttpStatusCode.NotFound).AsJsonResult();

			Stream s = file.CreateReadStream();

			string fileName = this.filenameDatabase.GetFilename(DownloadId);
			if (fileName != null) {
				fileName += Path.GetExtension(file.Path);
			}
			else {
				fileName = file.Path;
			}

			return File(s, "application/octet-stream", fileName);
		}
	}
}