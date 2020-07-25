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
using SERP.UI.AuthenticateService;
using Microsoft.AspNetCore.Http;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Core.Entities.UserManagement;
using SERP.UI.Models.Enum;

namespace SERP.UI.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<RoleMaster, int> _roleMasterRepo;

        public HomeController(ILogger<HomeController> logger, IGenericRepository<RoleMaster, int> roleRepo)
        {
            _logger = logger;
            _roleMasterRepo = roleRepo;
        }
        [CustomAuthenticate]
        public async Task<IActionResult> Index()
        {
            var roleId = HttpContext.Session.GetInt32("RoleId");
            var roleMaster = await _roleMasterRepo.GetSingle(x => x.Id == roleId);
            
            if(roleMaster.RoleName== (RoleEnum.Admin).ToString())
            {

            }
            else if(roleMaster.RoleName==(RoleEnum.Account).ToString())
            {

            }
            else if(roleMaster.RoleName==(RoleEnum.Librarian).ToString())
            {

            }
            else if(roleMaster.RoleName==(RoleEnum.Student).ToString())
            {
                return View("~/Views/Home/StudentIndex.cshtml");
            }
            else if(roleMaster.RoleName==(RoleEnum.SuperAdmin).ToString())
            {

            }
            else if(roleMaster.RoleName==(RoleEnum.Teacher).ToString())
            {

            }
           
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
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
