using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using TomiSoft.YoutubeDownloader;
using TomiSoft.YoutubeDownloader.BusinessLogic.Services;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Controllers {
	public class HomeController : Controller {
		private readonly IMediaDownloader dl;
		private readonly IMaintenanceService maintenanceService;

		public HomeController(IMediaDownloader dl, IMaintenanceService maintenanceService) {
			this.dl = dl;
			this.maintenanceService = maintenanceService;
		}

		public IActionResult Index() {
			ViewBag.MaintenanceRunning = maintenanceService.IsMaintenanceRunning;
			return View();
		}

		public async Task<IActionResult> VersionAsync() {
			string youtubeDlVersion = null;
			try {
				await Task.Run(() => youtubeDlVersion = dl.GetVersion());
			}
			catch { }

			return base.Json(new AppVersionInfo(youtubeDlVersion));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}