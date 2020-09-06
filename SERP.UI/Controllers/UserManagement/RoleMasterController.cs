using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Entities.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.UserManagement
{
    public class RoleMasterController : Controller
    {
        private readonly IGenericRepository<RoleMaster, int> _IRoleMasterRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public RoleMasterController(IGenericRepository<RoleMaster, int> roleMasterRepo, IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IRoleMasterRepo = roleMasterRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => PartialView("~/Views/UserManagement/_RoleMasterIndex.cshtml"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleMaster roleMasterEntity)
        {
            try
            {
                var response=await _IRoleMasterRepo.CreateEntity(roleMasterEntity);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            catch(Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }
        }

        public async Task<IActionResult> GetRoleList()
        {
            var models = await _IRoleMasterRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/UserManagement/_RoleMasterListPartial.cshtml", models);
        }

        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var model = await _IRoleMasterRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode(model, 1);
                await _IRoleMasterRepo.CreateNewContext();
                var response = await _IRoleMasterRepo.Update(deleteModel);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            catch(Exception ex)
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