using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.HRModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class LeaveAllotmentController : Controller
    {
        private readonly IGenericRepository<LeaveAllotment, int> _ILeaveAllotmentRepo;
        private readonly IGenericRepository<DesignationModel, int> _IDesignationRepo;
        private readonly IGenericRepository<LeaveMaster, int> _ILeaveRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public LeaveAllotmentController(IGenericRepository<LeaveAllotment,int> leaveAllotmentRepo,
                                        IGenericRepository<DesignationModel, int> designationRepo, 
                                        IGenericRepository<LeaveMaster, int> leaveRepo, 
                                        IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _ILeaveAllotmentRepo = leaveAllotmentRepo;
            _IDesignationRepo = designationRepo;
            _ILeaveRepo = leaveRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var leaveAllotment = await _ILeaveAllotmentRepo.GetSingle(x => x.Id == id);
                ViewBag.DesignationList = await _IDesignationRepo.GetList(x => x.IsActive == 1);
                ViewBag.LeaveDetails = await _ILeaveRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/EmplpoyeeLeave/_CreateLeaveAllotmentPartial.cshtml", leaveAllotment);
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
        public async Task<IActionResult> AllotLeave(LeaveAllotment model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var response = await _ILeaveAllotmentRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(response));
                }
                else
                {
                    var response = await _ILeaveAllotmentRepo.CreateEntity(model);
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

        public async Task<IActionResult> LeaveAllotmentDetails()
        {
            try
            {
                var models = (from LA in await _ILeaveAllotmentRepo.GetList(x => x.IsActive == 1)
                              join DS in await _IDesignationRepo.GetList(x => x.IsActive == 1)
                              on LA.DesignationId equals DS.Id
                              join LM in await _ILeaveRepo.GetList(x => x.IsActive == 1)
                              on LA.LeaveId equals LM.Id
                              select new LeaveAllotmentModel
                              {
                                  Id = LA.Id,
                                  LeaveName = LM.LeaveName,
                                  Designation = DS.Name,
                                  LeavePerMonth = LA.LeavePerMonth
                              }).ToList();

                return PartialView("~/Views/EmplpoyeeLeave/_LeaveAllotmentDetail.cshtml", models);
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

        public async Task<IActionResult> DeleteAllotment(int id)
        {
            try
            {
                var model = await _ILeaveAllotmentRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<LeaveAllotment>(model, 1);
                await _ILeaveAllotmentRepo.CreateNewContext();
                var response = await _ILeaveAllotmentRepo.Update(deleteModel);
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