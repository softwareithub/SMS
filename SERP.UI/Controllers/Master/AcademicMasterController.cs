using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SERP.UI.Controllers.Master
{
    [Route("Master/{controller}/{action}")]
    public class AcademicMasterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}