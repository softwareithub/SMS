using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class LeaveMasterController : Controller
    {
        private readonly IGenericRepository<LeaveMaster, int> _ILeaveMasterRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public LeaveMasterController(IGenericRepository<LeaveMaster,int> leaveMasterRepo,
                                     IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _ILeaveMasterRepo = leaveMasterRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var leaveDetail = await _ILeaveMasterRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/EmplpoyeeLeave/_CreateLeaveMasterPartial.cshtml", leaveDetail);
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
        public async Task<IActionResult> CreateLeave(LeaveMaster model)
        {
            if (model.Id > 0)
            {
                var updateResponse = await _ILeaveMasterRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(updateResponse));
            }
            else {
                var createResponse = await _ILeaveMasterRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(createResponse));
            }
        }

        public async Task<IActionResult> LeaveDetails()
        {
            try
            {
                var leaveDetails = await _ILeaveMasterRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/EmplpoyeeLeave/_LeaveDetailPartial.cshtml", leaveDetails);
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

        public async  Task<IActionResult> DeleteLeave(int id)
        {
            try
            {
                var leave = await _ILeaveMasterRepo.GetSingle(x => x.Id == id);
                leave.IsActive = 0;
                leave.IsDeleted = 1;
                await _ILeaveMasterRepo.CreateNewContext();
                var response = await _ILeaveMasterRepo.Update(leave);
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