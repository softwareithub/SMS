using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Extension;

namespace SERP.UI.Controllers.UserManagement
{
    public class AuthenticateController : Controller
    {
        private readonly IGenericRepository<Authenticate, int> _IAuthenticateRepo;
        private readonly IGenericRepository<ModuleMaster, int> _IModuleRepo;
        private readonly IGenericRepository<SubModuleMaster, int> _ISubModuleRepo;
        private readonly IGenericRepository<UserAccessRight, int> _IUserAccessRight;
        private readonly IGenericRepository<InstituteMaster, int> _IInstituteRepo;
        public AuthenticateController(IGenericRepository<Authenticate, int> authenticateRepo,
            IGenericRepository<ModuleMaster, int>  moduleRepo,
            IGenericRepository<SubModuleMaster, int> subModuleRepo,
            IGenericRepository<UserAccessRight, int> userAcccessRight,
            IGenericRepository<InstituteMaster, int> instituteRepo
            )
        {
            _IAuthenticateRepo = authenticateRepo;
            _IModuleRepo = moduleRepo;
            _ISubModuleRepo = subModuleRepo;
            _IUserAccessRight = userAcccessRight;
            _IInstituteRepo = instituteRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string message, string returnUrl)
        {
            ViewBag.message = message;
            var instituteModel = await _IInstituteRepo.GetSingle(x => x.IsActive == 1);
            ViewBag.Logo = instituteModel.InstituteLogo;
            ViewBag.Name = instituteModel.Name;
            ViewBag.Rythum = instituteModel.Rythum;
            HttpContext.Session.SetString("InstituteName", instituteModel.Name);
            HttpContext.Session.SetString("InstituteLogo", instituteModel.InstituteLogo);
            return await Task.Run(()=>View("~/Views/UserManagement/_LoginPartial.cshtml"));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Authenticate model)
        {
            var response = await _IAuthenticateRepo.GetSingle(x => x.UserName == model.UserName
            && x.Password == model.Password && x.IsActive == 1 && x.IsExpired == 0 && x.IsLocked == 0);
            string responseMessage = string.Empty;

            if(response!=null)
            {
                if(response.IsExpired==1)
                {
                    responseMessage = "User expired. please contact admin";
                    return RedirectToAction("Login", "Authenticate", new { message = responseMessage });
                }
                else if(response.IsLocked==1)
                {
                    responseMessage = "User locked. please contact admin";
                    return RedirectToAction("Login", "Authenticate", new { message = responseMessage });
                }
                else if(response.Attempt>3)
                {
                    responseMessage = "User attempted more than 3 times , your account has been locked";
                    return RedirectToAction("Login", "Authenticate", new { message = responseMessage });
                }
                else
                {
                    return await ValidatedUser(response);
                }
               
            }
            responseMessage = "Invalid user name and password.";
            return RedirectToAction("Login","Authenticate",new { message= responseMessage });
        }

        private async Task<IActionResult> ValidatedUser(Authenticate model)
        {
            HttpContext.Session.SetString("userName", model.UserName);
            var lastLogin = model.LastLoginDateTime;
            model.LastLoginDateTime = DateTime.Now;

            await _IAuthenticateRepo.CreateNewContext();

            var responseData = await _IAuthenticateRepo.Update(model);
            HttpContext.Session.SetString("UserName", model.UserName);

            HttpContext.Session.SetObject("menuSubMenu",await GetMenuSubMenu(model.EmployeeId));
            HttpContext.Session.SetInt32("EmployeeId", model.EmployeeId);

            return RedirectToAction("Index", "Home");
        }

        private async Task<List<MenuSubMenuVm>> GetMenuSubMenu(int employeeId)
        {
            HttpContext.Session.SetInt32("EmployeeId", employeeId);
            var result = (from UR in await _IUserAccessRight.GetList(x => x.EmployeeId == employeeId && x.IsActive == 1)
                          join MM in await _IModuleRepo.GetList(x => x.IsActive == 1)
                          on UR.ModuleId equals MM.Id
                          join SM in await _ISubModuleRepo.GetList(x => x.IsActive == 1)
                          on UR.SubModuleId equals SM.Id
                          select new MenuSubMenuVm
                          {
                              ModuleId= MM.Id,
                              ModuleName= MM.ModuleName,
                              ModuleClass= MM.ClassName,
                              SubModuleId= SM.Id,
                              SubModuleName= SM.SubModuleName,
                              SubModuleClass= SM.ClassName,
                              ActionName= SM.ActionName,
                              ControllerName= SM.ControllerName,
                              ModuleClassName= MM.ClassName,
                              ModuleDisplayOrder=  MM.DisplayOrder,
                              SubModuleDisplayOrder= SM.DisplayOrder
                          }).ToList();

            return result.OrderBy(x=>x.ModuleDisplayOrder).ThenBy(x=>x.SubModuleDisplayOrder).ToList();

        }
    }
}