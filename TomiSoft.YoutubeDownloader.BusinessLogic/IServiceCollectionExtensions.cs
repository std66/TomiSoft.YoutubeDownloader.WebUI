using Microsoft.Extensions.DependencyInjection;
using TomiSoft.Common.Hosting;
using TomiSoft.YoutubeDownloader.BusinessLogic.Data;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YouTubeDownloader.BusinessLogic.BusinessModels;
using TomiSoft.YouTubeDownloader.BusinessLogic.DataManagement;

namespace TomiSoft.YoutubeDownloader.BusinessLogic {
	public static class IServiceCollectionExtensions {
		public static IServiceCollection AddYoutubeDownloaderCore(this IServiceCollection services) {
			return services
				.AddBusinessLogicLayer()
				.AddDataManagementLayer();
		}

		public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services) {
			return services
				.AddSingleton<IQueueService<DownloadRequestBM>, QueueService<DownloadRequestBM>>()
				.AddSingleton<IDownloadManagementService, DownloadManagementService>();
		}

		public static IServiceCollection AddDataManagementLayer(this IServiceCollection services) {
			return services
				.AddSingleton<IPersistedServiceStatusDataManager, PersistedServiceStatusDataManager>()
				.AddSingleton<IFilenameDatabase, MemoryFilenameDB>();
		}
	}
}