using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TomiSoft.YouTubeDownloader.BackgroundDownloaderService.HostedServices;

namespace TomiSoft.YoutubeDownloader.BackgroundDownloaderService.HostedServices {
    public class BackgroundDownloaderHostedServiceAdapter : IHostedService {
        private readonly IHostedService service;

        public BackgroundDownloaderHostedServiceAdapter(IDownloaderService downloaderService) => this.service = (IHostedService)downloaderService;

        public Task StartAsync(CancellationToken cancellationToken) => service.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken) => service.StopAsync(cancellationToken);
    }
}
