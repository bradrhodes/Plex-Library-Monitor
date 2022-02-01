using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlexLibraryMonitor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PlexLibraryMonitor.Plex;

namespace PlexLibraryMonitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenerateAuthAppUrl _urlGenerator;

        public HomeController(ILogger<HomeController> logger, IGenerateAuthAppUrl urlGenerator)
        {
            _logger = logger;
            _urlGenerator = urlGenerator ?? throw new ArgumentNullException(nameof(urlGenerator));
        }

        public IActionResult Index()
        {
            var url = _urlGenerator.GenerateUrl("clientId", "pinCode", "clientName");
            _logger.LogInformation($"Generated URL: {url}");
            return View("Index", url);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
