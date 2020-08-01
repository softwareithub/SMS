using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.FrontOffice;
using SERP.Core.Model.FrontOffice;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.FrontOffice
{
    public class StudentGatePassController : Controller
    {
        private readonly IGenericRepository<GuardianMaster, int> _guardiamRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IGenericRepository<StudentGatePass, int> _studentGatePassRepo;

        public StudentGatePassController(IGenericRepository<GuardianMaster, int> guardianRepo, IGenericRepository<StudentMaster, int> studentRepo, IGenericRepository<StudentGatePass, int> studentGatePass)
        {
            _guardiamRepo = guardianRepo;
            _studentRepo = studentRepo;
            _studentGatePassRepo = studentGatePass;
        }
        public async Task<IActionResult> Index(int id)
        {
            var model = await _studentRepo.GetList(x => x.IsActive == 1);
            model.ToList().ForEach(item =>
            {
                item.Name = item.Name + "  (" + item.RegistrationNumber + ") ";
            });
            ViewBag.StudentList = model;
            var studentGatePassModel = await _studentGatePassRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/FrontOffice/_StudentGatePassPartial.cshtml", studentGatePassModel);
        }

        public async Task<IActionResult> GuardianList(int studentId)
        {
            var model = await _guardiamRepo.GetList(x => x.StudentId == studentId && x.IsActive == 1);
            return PartialView("~/Views/FrontOffice/_GuardianListPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentGatePass(StudentGatePass gatePass)
        {
            if (gatePass.Id == 0)
            {
                var response = await _studentGatePassRepo.CreateEntity(gatePass);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<StudentGatePass>(gatePass, 1);
                var response = await _studentGatePassRepo.Update(gatePass);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }

        public async Task<IActionResult> GetSudentGatePass()
        {
            var models = (from SG in await _studentGatePassRepo.GetList(x => x.IsActive == 1)
                          join SM in await _studentRepo.GetList(x => x.IsActive == 1)
                          on SG.StudentId equals SM.Id
                          join GM in await _guardiamRepo.GetList(x => x.IsActive == 1)
                          on SG.GuradianId equals GM.Id
                          select new StudentGatePassModel
                          {
                              Id = SG.Id,
                              StudentName = SM.Name + " (" + SM.RegistrationNumber + ") ",
                              GuardianName= GM.GuradianName,
                              ApprovedBy= SG.ApproveBy,
                              ReasonOfLeaving= SG.Reason,
                              LeavingTime= SG.StudentLeave
                          }).ToList();

            return PartialView("~/Views/FrontOffice/_GuardianListDetailPartial.cshtml", models);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _studentGatePassRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<StudentGatePass>(model, 1);
            await _studentGatePassRepo.CreateNewContext();
            var response = await _studentGatePassRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}