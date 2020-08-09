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
        private readonly IGenericRepository<Authenticate, int> _authenticateRepo;
        private readonly IGenericRepository<RoleMaster, int> _roleRepo;

        public UserAccessRightController(IGenericRepository<ModuleMaster, int> moduleRepo,
                                IGenericRepository<SubModuleMaster, int> subModuleRepo,
                                IGenericRepository<UserAccessRight, int> userAceessRepo, 
                                IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, IGenericRepository<Authenticate, int> authenticateRepo, IGenericRepository<RoleMaster, int> roleRepo)
        {
            _IModuleMasterRepo = moduleRepo;
            _ISubModuleRepo = subModuleRepo;
            _IUserAccessRepo = userAceessRepo;
            _IEmployeeRepo = employeeRepo;
            _authenticateRepo = authenticateRepo;
            _roleRepo = roleRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.roleList = await _roleRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/UserManagement/_UserEmployeePartial.cshtml");
        }
        public async Task<IActionResult> GetUserAccess(int roleId)
        {
            HttpContext.Session.SetInt32("RoleId", roleId);
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
            var userAccessList = await _IUserAccessRepo.GetList(x => x.RoleId == roleId && x.IsActive == 1);
            foreach(var data in userAccessList)
            {
                var model = result.Find(x => x.ModuleId == data.ModuleId && x.SubModuleId == data.SubModuleId);
                if(model!=null)
                {
                    model.CreateAccess = data.CreateAccess;
                    model.ReadAccess = data.ReadAccess;
                    model.UpdateAccess = data.UpdateAccess;
                }
               
            }

            return PartialView("~/Views/UserManagement/_UserAccessInformation.cshtml", result);
            
        }

        [HttpPost]
        public async Task<IActionResult> UserAccess(int [] module,int[] access)
        {
            int roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            //Get All the Submodules
            var subModuleDetails = await _ISubModuleRepo.GetList(x => x.IsActive == 1);

            List<int> moduleList = new List<int>();

            for(int i=0; i<access.Count(); i++)
            {
                moduleList.Add(subModuleDetails.Where(x => x.Id == access[i]).Select(x => x.ModuleId).FirstOrDefault());
            }

            var useRightModels = await _IUserAccessRepo.GetList(x => x.RoleId == roleId);
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
                model.ModuleId = moduleList[i];
                model.SubModuleId = access[i];
                model.RoleId = roleId;
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