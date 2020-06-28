using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class LeaveMasterController : Controller
    {
        private readonly IGenericRepository<LeaveMaster, int> _ILeaveMasterRepo;
        public LeaveMasterController(IGenericRepository<LeaveMaster,int> leaveMasterRepo)
        {
            _ILeaveMasterRepo = leaveMasterRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var leaveDetail = await _ILeaveMasterRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/EmplpoyeeLeave/_CreateLeaveMasterPartial.cshtml",leaveDetail);
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
            var leaveDetails = await _ILeaveMasterRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/EmplpoyeeLeave/_LeaveDetailPartial.cshtml", leaveDetails);
        }

        public async  Task<IActionResult> DeleteLeave(int id)
        {
            var leave = await _ILeaveMasterRepo.GetSingle(x => x.Id == id);
            leave.IsActive = 0;
            leave.IsDeleted = 1;
            await _ILeaveMasterRepo.CreateNewContext();
            var response = await _ILeaveMasterRepo.Update(leave);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}