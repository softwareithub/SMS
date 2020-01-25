using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Transaction.SubjectTransaction
{
    public class SubjectMasterController : Controller
    {
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly ISubjectMasterRepo _ISubjectRepo;

        public SubjectMasterController(IGenericRepository<BatchMaster, int> batchRepo, IGenericRepository<CourseMaster, int> courseRepo,
            ISubjectMasterRepo subjectRepo)
        {
            _IBatchRepo = batchRepo;
            _ICourseRepo = courseRepo;
            _ISubjectRepo = subjectRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            var result = await _ISubjectRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/SubjectMaster/_AddSubjectMasterIndexPartial.cshtml", result);
        }

        public async Task<IActionResult> AddSubjectPartial(int classId, int batchId)
        {
            TempData["CourseId"] = classId;
            TempData["BatchId"] = batchId;
            return await Task.Run(() => PartialView("~/Views/SubjectMaster/SubjectMasterPartial.cshtml"));
        }

        [HttpPost]
        public async Task<IActionResult> PostAddSubject(SubjectMaster model)
        {
            //Delete Records
            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var result = await _ISubjectRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                var result = await _ISubjectRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
        }

        public async Task<IActionResult> GetSubjectDatList()
        {
            var subjectModel = await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            var courseModel = await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

            var result = (from SM in subjectModel
                          join CM in courseModel
                          on SM.CourseId equals CM.Id
                          where CM.IsDeleted == 0 && CM.IsActive == 1
                          && SM.IsActive == 1 && SM.IsDeleted == 0
                          select new SubjectVm
                          {
                              SubjectId = SM.Id,
                              CourseName = CM.Name,
                              SubjectCode = SM.SubjectCode,
                              SubjectName = SM.SubjectName,
                              SubjectDescription = SM.SubjectDescription,
                              IsElective = SM.IsElective
                          }).ToList();

            return PartialView("~/Views/SubjectMaster/SubjectMasterPartial.cshtml", result);
        }

        #region PrivateFields
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subjectModel = await _ISubjectRepo.GetSingle(x => x.Id == id);
            subjectModel.IsActive = 0;
            subjectModel.IsDeleted = 1;
            subjectModel.UpdatedBy = 1;
            subjectModel.UpdatedDate = DateTime.Now.Date;
            await _ISubjectRepo.CreateNewContext();
            var result = await _ISubjectRepo.Delete(subjectModel);
            return Json(ResponseData.Instance.GenericResponse(result));
        }
        #endregion

    }
}