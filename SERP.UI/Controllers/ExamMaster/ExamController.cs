using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.ExamMaster
{
    public class ExamController : Controller
    {
        private readonly IGenericRepository<Exam, int> _examRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public ExamController(IGenericRepository<Exam, int> examRepo,
                              IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _examRepo = examRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int Id)
        {
            try
            {
                var result = await _examRepo.GetSingle(x => x.Id == Id);
                return await Task.Run(() => PartialView("~/Views/ExamMaster/_ExamPartial.cshtml", result));
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
        public async Task<IActionResult> CreateExam(Exam model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var result = await _examRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    var result = await _examRepo.Update(model);
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

        public async Task<IActionResult> GetExamList()
        {
            try
            {
                var result = await _examRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/ExamMaster/_ExamListPartial.cshtml", result);
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

        public async Task<IActionResult> DeleteExam(int Id)
        {
            try
            {
                var model = await _examRepo.GetSingle(x => x.Id == Id);
                model.IsActive = 0;
                model.IsDeleted = 1;
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                await _examRepo.CreateNewContext();
                var result = await _examRepo.Update(model);
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