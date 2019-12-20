using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SERP.UI.Controllers.Master
{
    public class InstituteMasterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}