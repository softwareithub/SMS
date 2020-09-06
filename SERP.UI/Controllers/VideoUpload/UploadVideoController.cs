using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SERP.UI.Controllers.VideoUpload
{
    public class UploadVideoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}