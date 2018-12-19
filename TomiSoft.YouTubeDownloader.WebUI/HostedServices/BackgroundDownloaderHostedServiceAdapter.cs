using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace TomiSoft.YouTubeDownloader.WebUI.HostedServices {
    public class BackgroundDownloaderHostedServiceAdapter : IHostedService {
        private readonly IHostedService service;

        public BackgroundDownloaderHostedServiceAdapter(IDownloaderService downloaderService) => this.service = (IHostedService)downloaderService;

        public Task StartAsync(CancellationToken cancellationToken) => service.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken) => service.StopAsync(cancellationToken);
    }
}
