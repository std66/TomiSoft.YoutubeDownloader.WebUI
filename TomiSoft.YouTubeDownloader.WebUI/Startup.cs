using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tomisoft.YoutubeDownloader;
using TomiSoft.YouTubeDownloader.WebUI.Core;
using TomiSoft.YouTubeDownloader.WebUI.Data;
using TomiSoft.YouTubeDownloader.WebUI.HostedServices;

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
            
            services.AddSingleton<IYoutubeDlConfiguration>(YoutubeConfiguration);
            services.AddSingleton<IDownloaderServiceConfiguration>(YoutubeConfiguration);
            services.AddSingleton<IFilenameDatabase, MemoryFilenameDB>();
            services.AddSingleton<IProcessFactory, ProcessFactory>();
            services.AddSingleton<IMediaDownloader, YoutubeDl>();
            
            services.AddSingleton<IDownloaderService, BackgroundDownloaderService>();
            services.AddHostedService<BackgroundDownloaderHostedServiceAdapter>();

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

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
