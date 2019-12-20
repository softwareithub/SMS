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
        [Route("Master/Academic/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}