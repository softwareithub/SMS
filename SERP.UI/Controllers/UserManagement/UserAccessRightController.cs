using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.UserManagement
{
    public class UserAccessRightController : Controller
    {
        private readonly IGenericRepository<ModuleMaster, int> _IModuleMasterRepo;
        private readonly IGenericRepository<SubModuleMaster, int> _ISubModuleRepo;
        private readonly IGenericRepository<UserAccessRight, int> _IUserAccessRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _IEmployeeRepo;

        public UserAccessRightController(IGenericRepository<ModuleMaster, int> moduleRepo,
                                IGenericRepository<SubModuleMaster, int> subModuleRepo,
                                IGenericRepository<UserAccessRight, int> userAceessRepo, 
                                IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo)
        {
            _IModuleMasterRepo = moduleRepo;
            _ISubModuleRepo = subModuleRepo;
            _IUserAccessRepo = userAceessRepo;
            _IEmployeeRepo = employeeRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.employees = await _IEmployeeRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/UserManagement/_UserEmployeePartial.cshtml");
        }
        public async Task<IActionResult> GetUserAccess(int employeeId)
        {
            HttpContext.Session.SetInt32("employeeId", employeeId);
            var result = (from MM in await _IModuleMasterRepo.GetList(x => x.IsActive == 1)
                          join SM in await _ISubModuleRepo.GetList(x => x.IsActive == 1)
                          on MM.Id equals SM.ModuleId
                          select new UserAccessRightModel
                          {
                              ModuleId= MM.Id,
                              SubModuleId= SM.Id,
                              ModuleName= MM.ModuleName,
                              SubModuleName= SM.SubModuleName,
                          }).ToList();
            var userAccessList = await _IUserAccessRepo.GetList(x => x.EmployeeId == employeeId && x.IsActive == 1);
            foreach(var data in userAccessList)
            {
                var model = result.Find(x => x.ModuleId == data.ModuleId && x.SubModuleId == data.SubModuleId);
                model.CreateAccess = data.CreateAccess;
                model.ReadAccess = data.ReadAccess;
                model.UpdateAccess = data.UpdateAccess;
            }

            return PartialView("~/Views/UserManagement/_UserAccessInformation.cshtml", result);
            
        }

        [HttpPost]
        public async Task<IActionResult> UserAccess(int [] module,int[] access)
        {
            int employeeId = Convert.ToInt32(HttpContext.Session.GetInt32("employeeId"));

            var useRightModels = await _IUserAccessRepo.GetList(x => x.EmployeeId == employeeId);
            await _IUserAccessRepo.CreateNewContext();

            useRightModels.ToList().ForEach(item => {
                item.IsActive = 0;
                item.IsDeleted = 1;
            });

            var updateResponse = await _IUserAccessRepo.Update(useRightModels.ToArray());
            await _IUserAccessRepo.CreateNewContext();

            List<UserAccessRight> models = new List<UserAccessRight>();
            for(int i=0; i< access.Count(); i++)
            {
                UserAccessRight model = new UserAccessRight();
                model.ModuleId = module[i];
                model.SubModuleId = access[i];
                model.EmployeeId = Convert.ToInt32(HttpContext.Session.GetInt32("employeeId"));
                model.ReadAccess = 1;
                model.CreateAccess = 1;
                model.UpdateAccess = 1;
                models.Add(model);
            }

            var response = await _IUserAccessRepo.Add(models.ToArray());
            return Json(ResponseData.Instance.GenericResponse(response));
        }

        
    }
}