using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PruebaConfiguracion.Models;
using System.Diagnostics;

namespace PruebaConfiguracion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private AppSetting _options;

        public HomeController(ILogger<HomeController> logger, IOptionsSnapshot<AppSetting> options, IConfiguration configuration)
        {
            _logger = logger;
            this._configuration = configuration;
            this._options = options.Value;
        }

        public IActionResult Index()
        {

            var section = _configuration["PEPE:NOMBRE"];
            _configuration.GetSection("PEPE:PEPE2").Bind(_options);
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
