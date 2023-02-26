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

        public async Task EnqueueDownload(string MediaUri, string MediaFormat) {
            MediaFormat targetFormat = YoutubeDownloader.Media.MediaFormat.Video;

            if (MediaFormat == "mp3audio") {
                targetFormat = YoutubeDownloader.Media.MediaFormat.MP3Audio;
            }

            Uri mediaUri = new Uri(MediaUri);

            DownloadRequestBM request = new DownloadRequestBM(mediaUri, targetFormat);

            downloadServiceQueue.Enqueue(request);
            this.filenameDatabase.AssignDownloadId(mediaUri, request.DownloadId);

            await Groups.AddToGroupAsync(Context.ConnectionId, request.DownloadId.ToString());
            await Clients.Caller.SendAsync("UseDownloadId", request.DownloadId);
        }

        public async Task RequestProgress(string DownloadId) {
            if (Guid.TryParse(DownloadId, out Guid downloadGuid)) {
                DownloadStatusBM progress = this.downloaderService.GetDownloadStatus(downloadGuid);

                await Clients
                    .Caller
                    .SendAsync(
                        "UpdateDownloadStatus",
                        new UpdateDownloadStatus(progress.Status.ToString(), progress.Percentage)
                    );
            }
        }
    }
}
