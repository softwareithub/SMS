using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.ExamMaster
{
    public class GradeMasterController : Controller
    {
        private readonly IGenericRepository<GradeMaster, int> _IGradeMasterRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public GradeMasterController(IGenericRepository<GradeMaster, int> _gradeMasterRepo,
                                     IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IGradeMasterRepo = _gradeMasterRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> GradeList()
        {
            try
            {
                var result = await _IGradeMasterRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/ExamMaster/_GradeMasterPartial.cshtml", result);
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

        public async Task<IActionResult> CreateGrade(int id )
        {
            try
            {
                var response = await _IGradeMasterRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/ExamMaster/_CreateGradeMasterPartial.cshtml", response);
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
        public async Task<IActionResult> CreateGrade(GradeMaster model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.IsActive = 1;
                    model.IsDeleted = 0;
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now;
                    var response = await _IGradeMasterRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(response));

                }
                else
                {
                    var response = await _IGradeMasterRepo.CreateEntity(model);
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
        public async Task<IActionResult> DeleteGrade(int id)
        {
            try
            {
                var response = await _IGradeMasterRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<GradeMaster>(response, 1);
                await _IGradeMasterRepo.CreateNewContext();
                var responseData = await _IGradeMasterRepo.Update(deleteModel);
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