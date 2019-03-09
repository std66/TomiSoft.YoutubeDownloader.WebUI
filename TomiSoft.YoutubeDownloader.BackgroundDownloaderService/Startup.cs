using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TomiSoft.Common.FileManagement;
using TomiSoft.Common.SystemClock;
using TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core;
using TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core.Media;
using TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Core.ServiceState;
using TomiSoft.YoutubeDownloader.BackgroundDownloaderService.Data;
using TomiSoft.YoutubeDownloader.BackgroundDownloaderService.HostedServices;
using TomiSoft.YouTubeDownloader.BackgroundDownloaderService.HostedServices;

namespace TomiSoft.YoutubeDownloader.BackgroundDownloaderService {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private YoutubeConfiguration YoutubeConfiguration {
            get {
                YoutubeConfiguration config = new YoutubeConfiguration();
                Configuration.GetSection("YoutubeConfiguration").Bind(config);

                return config;
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IYoutubeDlConfiguration>(YoutubeConfiguration);
            services.AddSingleton<IDownloaderServiceConfiguration>(YoutubeConfiguration);
            services.AddSingleton<ISystemClock, SystemClock>();
            services.AddSingleton<IFilenameDatabase, MemoryFilenameDB>();
            services.AddSingleton<IProcessFactory, ProcessFactory>();
            services.AddSingleton<IMediaDownloader, YoutubeDl>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddSingleton<ITagInfoWriter, TaglibSharpTagInfoWriter>();
            services.AddSingleton<IDownloaderServiceState, DownloaderServiceState>();

            services.AddSingleton<IDownloaderService, HostedServices.BackgroundDownloaderService>();
            services.AddHostedService<BackgroundDownloaderHostedServiceAdapter>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
