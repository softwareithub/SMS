using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.UserManagement
{
    public class SubMenuController : Controller
    {
        private readonly IGenericRepository<SubModuleMaster, int> _ISubModuleRepo;
        private readonly IGenericRepository<ModuleMaster, int> _IModuleRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;


        public SubMenuController(IGenericRepository<SubModuleMaster, int> subModuleRepo,
                                 IGenericRepository<ModuleMaster, int> moduleRepo,
                                  IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _ISubModuleRepo = subModuleRepo;
            _IModuleRepo = moduleRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;

        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.Modules = await _IModuleRepo.GetList(x => x.IsActive == 1);
                var model = await _ISubModuleRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/UserManagement/SubModulePartial.cshtml", model);
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

        public async Task<IActionResult> CreateSubModule(SubModuleMaster model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now.Date;
                    var response = await _ISubModuleRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(response));
                }
                else
                {
                    var response = await _ISubModuleRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(response));
                }
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

        public async Task<IActionResult> SubModuleList()
        {
            try
            {
                var result = (from MM in await _IModuleRepo.GetList(x => x.IsActive == 1)
                              join SM in await _ISubModuleRepo.GetList(x => x.IsActive == 1)
                              on MM.Id equals SM.ModuleId
                              select new UserAccessRightModel
                              {
                                  ModuleId = MM.Id,
                                  SubModuleId = SM.Id,
                                  ModuleName = MM.ModuleName,
                                  SubModuleName = SM.SubModuleName,
                                  ControllerName = SM.ControllerName,
                                  ActionName = SM.ActionName,
                              }).ToList();
                return PartialView("~/Views/UserManagement/_SubModuleListPartial.cshtml", result);
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _ISubModuleRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<SubModuleMaster>(response, 1);
                await _ISubModuleRepo.CreateNewContext();
                var responseData = await _ISubModuleRepo.Update(deleteModel);
                return Json(ResponseData.Instance.GenericResponse(responseData));
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