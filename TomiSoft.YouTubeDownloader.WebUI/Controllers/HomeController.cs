﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TomiSoft.YouTubeDownloader.WebUI.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
