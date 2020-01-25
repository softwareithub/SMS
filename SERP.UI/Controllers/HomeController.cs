using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SERP.UI.Models;
using System.Net;
using Newtonsoft.Json;

namespace SERP.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //var url = "https://newsapi.org/v2/top-headlines?" + "country=in&" + "apiKey=151490ca09f645a4b25f580af42584e9";
            //var json = new WebClient().DownloadString(url);
            //JsonConvert.DeserializeObject<RootObject>(json);
            var dataModel = new List<RootObject>();
            return await  Task.Run(()=>View(dataModel)) ;
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
