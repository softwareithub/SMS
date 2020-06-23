using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SERP.UI.Controllers.Documents
{
    public class EmployeeDocumentController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => PartialView("~/Views/Documents/_EmployeeJoiningPartial.cshtml"));
        }
    }
}