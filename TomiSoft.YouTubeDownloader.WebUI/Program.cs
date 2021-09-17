using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using System;
using TomiSoft.Common.Configuration.ConfigMapFileProvider;

namespace TomiSoft.YouTubeDownloader.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    builder =>
                    {
                        builder
                            .AddJsonFile(
                                path: "appsettings.json",
                                optional: false,
                                reloadOnChange: true
                            )
                            .AddJsonFile(
                                ConfigMapFileProvider.FromRelativePath("config"),
                                "appsettings.json",
                                optional: true,
                                reloadOnChange: true
                            );
                    }
                )
                .UseStartup<Startup>()
                .UseNLog();
    }
}
