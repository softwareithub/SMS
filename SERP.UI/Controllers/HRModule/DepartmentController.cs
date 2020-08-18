using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.HRModule
{
    public class DepartmentController : Controller
    {
        private readonly IGenericRepository<DepartmentModel, int> _departmentRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public DepartmentController(IGenericRepository<DepartmentModel, int> departmentRepo,
                                    IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
            )
        {
            _departmentRepo = departmentRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var result = await _departmentRepo.GetSingle(x => x.Id == id);
                return await Task.Run(() => PartialView("~/Views/HRModule/_DepartmentCreatePartial.cshtml", result));
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
        public async Task<IActionResult> CreateDepartment(DepartmentModel model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var result = await _departmentRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now.Date;
                    var result = await _departmentRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
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

        public async Task<IActionResult> GetDepartmentList()
        {
            try
            {
                var result = await _departmentRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/HRModule/_DepartmentListPartial.cshtml", result);
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

        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var departmentModel = await _departmentRepo.GetSingle(x => x.Id == id);
                departmentModel.IsActive = 0;
                departmentModel.IsDeleted = 1;
                departmentModel.UpdatedBy = 1;
                departmentModel.UpdatedDate = DateTime.Now.Date;

                await _departmentRepo.CreateNewContext();

                var result = await _departmentRepo.Delete(departmentModel);

                return Json(ResponseData.Instance.GenericResponse(result));
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