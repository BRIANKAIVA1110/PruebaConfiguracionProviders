using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaConfiguracion.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using PepeConfiguration;

namespace PruebaConfiguracion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppSetting _options;

        public HomeController(ILogger<HomeController> logger, IOptions<AppSetting> options, IConfiguration configuration)
        {
            _logger = logger;
            this._configuration = configuration;
            this._options = options.Value;
        }

        public IActionResult Index()
        {
            
            var section = _configuration["PEPE:NOMBRE"];
            var section2 = _configuration["IIS_USER_HOME"];
            var section3 = _configuration["ASPNETCORE_ENVIRONMENT"];
            
            var conf = this._options;

            ViewBag.Data = section;

            return View();
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
