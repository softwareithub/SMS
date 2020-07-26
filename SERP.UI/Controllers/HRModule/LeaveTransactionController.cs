﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Model.HRModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class LeaveTransactionController : Controller
    {
        private readonly IGenericRepository<LeaveTransactionModel, int> _ILeaveTransactionRepo;
        private readonly IGenericRepository<LeaveMaster, int> _ILeaveMasterRepo;

        public LeaveTransactionController(IGenericRepository<LeaveTransactionModel, int> leaveTransactionRepo, IGenericRepository<LeaveMaster, int> leaveMasterRepo)
        {
            _ILeaveTransactionRepo = leaveTransactionRepo;
            _ILeaveMasterRepo = leaveMasterRepo;
        }
        public async Task<IActionResult> Index()
        {
            int employeeId =Convert.ToInt32(HttpContext.Session.GetInt32("EmployeeId"));
            var leaveTransactionDetails = (from LT in await _ILeaveTransactionRepo
                                          .GetList(x => x.EmployeeId == employeeId && x.IsActive == 1)
                                           join LM in await _ILeaveMasterRepo.GetList(x => x.IsActive == 1)
                                           on LT.LeaveId equals LM.Id
                                           select new LeaveTransactionVm
                                           {
                                               LeaveId= LT.LeaveId,
                                               LeaveName= LM.LeaveName,
                                               FromDate= LT.FromDate,
                                               ToDate= LT.ToDate,
                                               ReasonForLeave=LT.ReasonForLeave,
                                               ApproveDate= Convert.ToDateTime(LT.ApproveDate),
                                               RejectDate=Convert.ToDateTime(LT.RejectDate),
                                               RejectReason= LT.RejectReason,
                                               LeaveStatus= LT.LeaveStatus
                                               
                                           }).ToList();


            return PartialView("~/Views/HRModule/_ApplyLeaveDetailPartial.cshtml", leaveTransactionDetails);
        }

        public async Task<IActionResult> ApplyLeave(int id)
        {
            int employeeId = Convert.ToInt32(HttpContext.Session.GetInt32("EmployeeId"));
            ViewBag.LeaveList = await _ILeaveMasterRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/HRModule/_ApplyLeavePartial.cshtml");
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