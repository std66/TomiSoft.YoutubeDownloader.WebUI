﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;
using TomiSoft.Common.FileManagement;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.BusinessLogic;
using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YouTubeDownloader.WebUI.Configuration;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.Core.Media;
using TomiSoft.YouTubeDownloader.WebUI.DataRetention;
using TomiSoft.YouTubeDownloader.WebUI.HostedServices;
using TomiSoft.YouTubeDownloader.WebUI.Hubs;
using TomiSoft.YouTubeDownloader.WebUI.Metrics;

namespace TomiSoft.YouTubeDownloader.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            MetricServerConfiguration config = new MetricServerConfiguration();
            configuration.GetSection("Metrics").Bind(config);
            MetricsConfiguration = config;

            YoutubeConfiguration ytconfig = new YoutubeConfiguration();
            Configuration.GetSection("YoutubeConfiguration").Bind(ytconfig);
            YoutubeConfiguration = ytconfig;

            DataRetentionConfiguration dataRetentionConfiguration = new DataRetentionConfiguration();
            Configuration.GetSection("DataRetention").Bind(dataRetentionConfiguration);
            DataRetentionConfiguration = dataRetentionConfiguration;
        }

        public IConfiguration Configuration { get; }

        private YoutubeConfiguration YoutubeConfiguration { get; }

        private MetricServerConfiguration MetricsConfiguration { get; }
        public DataRetentionConfiguration DataRetentionConfiguration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddSingleton<IMediaDownloader, YoutubeDl>();

            if (MetricsConfiguration.Enabled)
            {
                services.AddMetricServer(options =>
                {
                    options.Url = MetricsConfiguration.Uri;
                    options.Port = MetricsConfiguration.Port;
                });

                services
                    .AddSingleton<IMediaDownloadMetrics, MediaDownloadMetrics>()
                    .Decorate<IMediaDownloader, MediaDownloaderMetricsDecorator>();
            }

            if (DataRetentionConfiguration.Enabled) {
                services
                    .AddSingleton<IDownloadedFileCleanupService, DownloadedFileCleanupService>()
                    .AddSingleton<IDataRetentionConfiguration>(DataRetentionConfiguration)
                    .AddHostedService<CleanupHostedService>()
                    .Decorate<IMediaDownloader, MediaDownloaderDataRetentionDecorator>();
            }

            services
                .AddYoutubeDownloaderCore();

            services
                .AddSingleton<IYoutubeDlConfiguration>(YoutubeConfiguration)
                .AddSingleton<IDownloaderServiceConfiguration>(YoutubeConfiguration)
                .AddSingleton<IProcessFactory, ProcessFactory>()
                .AddSingleton<IFileManager, FileManager>()
                .AddSingleton<ITagInfoWriter, TaglibSharpTagInfoWriter>()
                .AddSingleton<IDownloadStatusNotifier, DownloadStatusNotifier>();

            services
                .AddHostedService<MaintenanceHostedService>()
                .AddHostedService<BackgroundDownloaderService>();

            services.AddSignalR();
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app
                .UseHttpMetrics()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseRouting()
                .UseEndpoints(builder =>
                {
                    if (MetricsConfiguration.Enabled)
                    {
                        builder.MapMetrics();
                        log.LogInformation("Metric collection is enabled on port '{Port}' and path '{Uri}'", MetricsConfiguration.Port, MetricsConfiguration.Uri);
                    }
                    else
                    {
                        log.LogInformation("Metric collection is disabled in configuration.");
                    }

                    builder.MapHub<DownloadHub>("/downloadHub");
                })
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
