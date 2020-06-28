using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Model.HRModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class LeaveAllotmentController : Controller
    {
        private readonly IGenericRepository<LeaveAllotment, int> _ILeaveAllotmentRepo;
        private readonly IGenericRepository<DesignationModel, int> _IDesignationRepo;
        private readonly IGenericRepository<LeaveMaster, int> _ILeaveRepo;
        public LeaveAllotmentController(IGenericRepository<LeaveAllotment,int> leaveAllotmentRepo, IGenericRepository<DesignationModel, int> designationRepo, IGenericRepository<LeaveMaster, int> leaveRepo)
        {
            _ILeaveAllotmentRepo = leaveAllotmentRepo;
            _IDesignationRepo = designationRepo;
            _ILeaveRepo = leaveRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var leaveAllotment = await _ILeaveAllotmentRepo.GetSingle(x => x.Id == id);
            ViewBag.DesignationList = await _IDesignationRepo.GetList(x => x.IsActive == 1);
            ViewBag.LeaveDetails = await _ILeaveRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/EmplpoyeeLeave/_CreateLeaveAllotmentPartial.cshtml", leaveAllotment);
        }
        [HttpPost]
        public async Task<IActionResult> AllotLeave(LeaveAllotment model)
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

        public async Task<IActionResult> LeaveAllotmentDetails()
        {
            var models = (from LA in await _ILeaveAllotmentRepo.GetList(x => x.IsActive == 1)
                         join DS in await _IDesignationRepo.GetList(x => x.IsActive == 1)
                         on LA.DesignationId equals DS.Id
                         join LM in await _ILeaveRepo.GetList(x => x.IsActive == 1)
                         on LA.LeaveId equals LM.Id
                         select new LeaveAllotmentModel
                         { 
                            Id= LA.Id,
                            LeaveName= LM.LeaveName,
                            Designation= DS.Name,
                            LeavePerMonth= LA.LeavePerMonth
                         }).ToList();

             return PartialView("~/Views/EmplpoyeeLeave/_LeaveAllotmentDetail.cshtml", models);
        }

        public async Task<IActionResult> DeleteAllotment(int id)
        {
            var model = await _ILeaveAllotmentRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<LeaveAllotment>(model, 1);
            await _ILeaveAllotmentRepo.CreateNewContext();
            var response = await _ILeaveAllotmentRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}