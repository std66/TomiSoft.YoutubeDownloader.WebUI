using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;
using TomiSoft.Common.Configuration.ConfigMapFileProvider;
using NLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace TomiSoft.YouTubeDownloader.WebUI {
	public class Program {
		public static void Main(string[] args) {
			// NLog: setup the logger first to catch all errors
			string nlogConfig = File.Exists(Path.Combine("config", "nlog.config")) ? Path.Combine("config", "nlog.config") : "nlog.config";
			LogManager.Setup().LoadConfigurationFromFile(nlogConfig);
			var logger = LogManager.GetCurrentClassLogger();
			logger.Info("NLog is configured using '{ConfigFile}'", nlogConfig);

			try {
				logger.Info("Initializing WebHost...");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex) {
				//NLog: catch setup errors
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally {
				logger.Info("Application terminated.");
				// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
				NLog.LogManager.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureHostOptions(options => {
					options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
				})
				.ConfigureAppConfiguration(
					builder => {
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
				.ConfigureLogging(logging => {
					logging.ClearProviders();
				})
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<Startup>();
					webBuilder.UseNLog();
				});
	}
}