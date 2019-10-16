using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TomiSoft.Common.FileManagement;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.BusinessLogic;
using TomiSoft.YoutubeDownloader.BusinessLogic.Configuration;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.Core.Media;
using TomiSoft.YouTubeDownloader.WebUI.HostedServices;
using TomiSoft.YouTubeDownloader.WebUI.Hubs;

namespace TomiSoft.YouTubeDownloader.WebUI {
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

            services
                .AddYoutubeDownloaderCore();
            
            services
                .AddSingleton<IYoutubeDlConfiguration>(YoutubeConfiguration)
                .AddSingleton<IDownloaderServiceConfiguration>(YoutubeConfiguration)
                .AddSingleton<IProcessFactory, ProcessFactory>()
                .AddSingleton<IMediaDownloader, YoutubeDl>()
                .AddSingleton<IFileManager, FileManager>()
                .AddSingleton<ITagInfoWriter, TaglibSharpTagInfoWriter>()
                .AddSingleton<IDownloadStatusNotifier, DownloadStatusNotifier>();

            services
                .AddHostedService<CleanupHostedService>()
                .AddHostedService<MaintenanceHostedService>()
                .AddHostedService<BackgroundDownloaderService>();

            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSignalR(builder => {
                builder.MapHub<DownloadHub>("/downloadHub");
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
