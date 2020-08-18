using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
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
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        

        public UserAccessRightController(IGenericRepository<ModuleMaster, int> moduleRepo,
                                IGenericRepository<SubModuleMaster, int> subModuleRepo,
                                IGenericRepository<UserAccessRight, int> userAceessRepo, 
                                IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, 
                                IGenericRepository<Authenticate, int> authenticateRepo, 
                                IGenericRepository<RoleMaster, int> roleRepo,
                                IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IModuleMasterRepo = moduleRepo;
            _ISubModuleRepo = subModuleRepo;
            _IUserAccessRepo = userAceessRepo;
            _IEmployeeRepo = employeeRepo;
            _authenticateRepo = authenticateRepo;
            _roleRepo = roleRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.roleList = await _roleRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/UserManagement/_UserEmployeePartial.cshtml");
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }
        public async Task<IActionResult> GetUserAccess(int roleId)
        {
            try
            {
                HttpContext.Session.SetInt32("RoleId", roleId);
                var result = (from MM in await _IModuleMasterRepo.GetList(x => x.IsActive == 1)
                              join SM in await _ISubModuleRepo.GetList(x => x.IsActive == 1)
                              on MM.Id equals SM.ModuleId
                              select new UserAccessRightModel
                              {
                                  ModuleId = MM.Id,
                                  SubModuleId = SM.Id,
                                  ModuleName = MM.ModuleName,
                                  SubModuleName = SM.SubModuleName,
                              }).ToList();
                var userAccessList = await _IUserAccessRepo.GetList(x => x.RoleId == roleId && x.IsActive == 1);
                foreach (var data in userAccessList)
                {
                    var model = result.Find(x => x.ModuleId == data.ModuleId && x.SubModuleId == data.SubModuleId);
                    if (model != null)
                    {
                        model.CreateAccess = data.CreateAccess;
                        model.ReadAccess = data.ReadAccess;
                        model.UpdateAccess = data.UpdateAccess;
                    }

                }

                return PartialView("~/Views/UserManagement/_UserAccessInformation.cshtml", result);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> UserAccess(int [] module,int[] access)
        {
            try
            {
                int roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

                //Get All the Submodules
                var subModuleDetails = await _ISubModuleRepo.GetList(x => x.IsActive == 1);

                List<int> moduleList = new List<int>();

                for (int i = 0; i < access.Count(); i++)
                {
                    moduleList.Add(subModuleDetails.Where(x => x.Id == access[i]).Select(x => x.ModuleId).FirstOrDefault());
                }

                var useRightModels = await _IUserAccessRepo.GetList(x => x.RoleId == roleId);
                await _IUserAccessRepo.CreateNewContext();

                useRightModels.ToList().ForEach(item =>
                {
                    item.IsActive = 0;
                    item.IsDeleted = 1;
                });

                var updateResponse = await _IUserAccessRepo.Update(useRightModels.ToArray());
                await _IUserAccessRepo.CreateNewContext();

                List<UserAccessRight> models = new List<UserAccessRight>();
                for (int i = 0; i < access.Count(); i++)
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
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }

        
    }
}