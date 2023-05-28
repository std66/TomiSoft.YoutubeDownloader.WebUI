using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TomiSoft.Common.Hosting;
using TomiSoft.YoutubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YoutubeDownloader.Media;
using TomiSoft.YouTubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YouTubeDownloader.BusinessLogic.DataManagement;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Hubs {
	public class DownloadHub : Hub {
		private readonly IQueueService<DownloadRequestBM> downloadServiceQueue;
		private readonly IFilenameDatabase filenameDatabase;
		private readonly IDownloadManagementService downloaderService;

		public DownloadHub(IQueueService<DownloadRequestBM> queue, IFilenameDatabase filenameDatabase, IDownloadManagementService downloaderService) {
			this.downloadServiceQueue = queue;
			this.filenameDatabase = filenameDatabase;
			this.downloaderService = downloaderService;
		}

		public async Task EnqueueDownload(string mediaUri, string mediaFormat) {
			MediaFormat targetFormat = YoutubeDownloader.Media.MediaFormat.Video;

			if (mediaFormat == "mp3audio") {
				targetFormat = YoutubeDownloader.Media.MediaFormat.MP3Audio;
			}

			Uri uri = new Uri(mediaUri);

			DownloadRequestBM request = new DownloadRequestBM(uri, targetFormat);

			downloadServiceQueue.Enqueue(request);
			this.filenameDatabase.AssignDownloadId(uri, request.DownloadId);

			await Groups.AddToGroupAsync(Context.ConnectionId, request.DownloadId.ToString());
			await Clients.Caller.SendAsync("UseDownloadId", request.DownloadId);
		}

		public async Task RequestProgress(string downloadId) {
			if (!Guid.TryParse(downloadId, out Guid downloadGuid))
				return;

			if (!this.downloaderService.TryGetDownloadStatus(downloadGuid, out DownloadStatusBM progress))
				return;

			await Clients
				.Caller
				.SendAsync(
					"UpdateDownloadStatus",
					new UpdateDownloadStatus(progress.Status.ToString(), progress.Percentage)
				);
		}
	}
}