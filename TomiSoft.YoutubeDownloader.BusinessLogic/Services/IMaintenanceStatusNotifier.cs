namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
	public interface IMaintenanceStatusNotifier {
		void NotifyMaintenanceStart();
		void NotifyMaintenanceComplete();
	}
}