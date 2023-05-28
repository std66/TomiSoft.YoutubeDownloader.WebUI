using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;
using TomiSoft.Common.Configuration.ConfigMapFileProvider;

namespace TomiSoft.YouTubeDownloader.WebUI {
	public class Program {
		public static void Main(string[] args) {
			// NLog: setup the logger first to catch all errors
			string nlogConfig = File.Exists(Path.Combine("config", "nlog.config")) ? Path.Combine("config", "nlog.config") : "nlog.config";
			var logger = NLogBuilder.ConfigureNLog(nlogConfig).GetCurrentClassLogger();
			logger.Info("NLog is configured using '{ConfigFile}'", nlogConfig);

			try {
				logger.Info("Initializing WebHost...");
				CreateWebHostBuilder(args, nlogConfig).Build().Run();
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

		public static IWebHostBuilder CreateWebHostBuilder(string[] args, string nlogConfig) =>
			WebHost.CreateDefaultBuilder(args)
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
				.UseStartup<Startup>()
				.UseNLog();
	}
}