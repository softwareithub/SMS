using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.ExamMaster
{
    public class ExamUdpateController : Controller
    {
        private readonly IGenericRepository<ExamUpdate, int> _IExamUpdateRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public ExamUdpateController (IGenericRepository<ExamUpdate, int> examUpdateRepo,
                                     IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IExamUpdateRepo = examUpdateRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var response = await _IExamUpdateRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/ExamMaster/_ExamUpdatePartial.cshtml", response);
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
        public async Task<IActionResult> CreateEvent(ExamUpdate model)
        {
            if (model.Id == 0)
            {
                var result = await _IExamUpdateRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                var result = await _IExamUpdateRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEventDetails()
        {
            try
            {
                var responseDetails = await _IExamUpdateRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/ExamMaster/_ExamEventDetails.cshtml", responseDetails);
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

        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var responseData = await _IExamUpdateRepo.GetSingle(x => x.Id == id);
            responseData.IsActive = 0;
            responseData.IsDeleted = 1;
            await _IExamUpdateRepo.CreateNewContext();
            var response = await _IExamUpdateRepo.Update(responseData);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}