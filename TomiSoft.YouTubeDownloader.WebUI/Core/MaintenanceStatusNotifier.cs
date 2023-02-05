using Microsoft.AspNetCore.SignalR;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YouTubeDownloader.WebUI.Hubs;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
    public class MaintenanceStatusNotifier : IMaintenanceStatusNotifier {
        private readonly IHubContext<DownloadHub> hubContext;

        public MaintenanceStatusNotifier(IHubContext<DownloadHub> hubContext) {
            this.hubContext = hubContext;
        }

        public void NotifyMaintenanceComplete() {
            hubContext.Clients.All.SendAsync("MaintenanceCompleted").ConfigureAwait(false);
        }

        public void NotifyMaintenanceStart() {
            hubContext.Clients.All.SendAsync("MaintenanceStarted").ConfigureAwait(false);
        }
    }
}
