using System.Threading.Tasks;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Services {
    public interface IMaintenanceService {
        bool IsMaintenanceRunning { get; }

        Task RunMaintenanceAsync();
    }
}