using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class LeaveTransactionController : Controller
    {
        private readonly IGenericRepository<LeaveTransactionModel, int> _ILeaveTransactionRepo;
        private readonly IGenericRepository<LeaveMaster, int> _ILeaveMasterRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public LeaveTransactionController(IGenericRepository<LeaveTransactionModel, int> leaveTransactionRepo, 
                                          IGenericRepository<LeaveMaster, int> leaveMasterRepo,
                                          IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _ILeaveTransactionRepo = leaveTransactionRepo;
            _ILeaveMasterRepo = leaveMasterRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                int employeeId = Convert.ToInt32(HttpContext.Session.GetInt32("EmployeeId"));
                var leaveTransactionDetails = (from LT in await _ILeaveTransactionRepo
                                              .GetList(x => x.EmployeeId == employeeId && x.IsActive == 1)
                                               join LM in await _ILeaveMasterRepo.GetList(x => x.IsActive == 1)
                                               on LT.LeaveId equals LM.Id
                                               select new LeaveTransactionVm
                                               {
                                                   LeaveId = LT.LeaveId,
                                                   LeaveName = LM.LeaveName,
                                                   FromDate = LT.FromDate,
                                                   ToDate = LT.ToDate,
                                                   ReasonForLeave = LT.ReasonForLeave,
                                                   ApproveDate = Convert.ToDateTime(LT.ApproveDate),
                                                   RejectDate = Convert.ToDateTime(LT.RejectDate),
                                                   RejectReason = LT.RejectReason,
                                                   LeaveStatus = LT.LeaveStatus

                                               }).ToList();


                return PartialView("~/Views/HRModule/_ApplyLeaveDetailPartial.cshtml", leaveTransactionDetails);
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

        public async Task<IActionResult> ApplyLeave(int id)
        {
            try
            {
                int employeeId = Convert.ToInt32(HttpContext.Session.GetInt32("EmployeeId"));
                ViewBag.LeaveList = await _ILeaveMasterRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/HRModule/_ApplyLeavePartial.cshtml");
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
        public async Task<IActionResult> ApplyLeaveEmployee(LeaveTransactionModel model)
        {
            int employeeId = Convert.ToInt32(HttpContext.Session.GetInt32("EmployeeId"));
            model.EmployeeId = employeeId;
            if (model.Id == 0)
            {
                model.LeaveStatus = "Pending";
                var response = await _ILeaveTransactionRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<LeaveTransactionModel>(model, employeeId);
                var response = await _ILeaveTransactionRepo.Update(updateModel);
                return Json(ResponseData.Instance.GenericResponse(response));
                
            }
        }
    }
}