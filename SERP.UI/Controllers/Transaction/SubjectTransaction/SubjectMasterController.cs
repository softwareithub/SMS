using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
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
        public async Task<IActionResult> Index()
        {
            ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return await Task.Run(() => PartialView("~/Views/SubjectMaster/_AddSubjectMasterIndexPartial.cshtml"));
        }

        public async Task<IActionResult> AddSubjectPartial(int classId, int batchId)
        {
            TempData["CourseId"] = classId;
            TempData["BatchId"] = batchId;
            return await Task.Run(() => PartialView("~/Views/SubjectMaster/SubjectMasterPartial.cshtml"));
        }

        [HttpPost]
        public async Task<IActionResult> PostAddSubject(string[] subjectCodes, string[] subjectNames, string[] subjectDescriptions, int[] ids)
        {
            int courseId = Convert.ToInt32(TempData["CourseId"]);
            int batchId = Convert.ToInt32(TempData["BatchId"]);

            //Delete Records
            if (ids.Count() > 0)
            {
                await _ISubjectRepo.DeleteMultipleSubject(ids.ToList());
            }

            //To Insert multiple records
            for (int i = 0; i < subjectCodes.Count(); i++)
            {
                var model = new SubjectMaster()
                {
                    BatchId = batchId,
                    CourseId = courseId,
                    SubjectCode = subjectCodes[i],
                    SubjectName = subjectNames[i],
                    SubjectDescription = string.IsNullOrEmpty(subjectDescriptions[i]) ? string.Empty : subjectDescriptions[i],
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now.Date
                };

                var result = await _ISubjectRepo.CreateEntity(model);
            }

            return Json("Subject Added Successfully");
        }

        public async Task<IActionResult> GetSubjectDatList(int courseId, int batchId)
        {
            var result = await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.CourseId == courseId && x.BatchId == batchId);
            return Json(result);
        }

        #region PrivateFields
        private async Task<ResponseStatus> DeleteSubject(int[] ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                var subjectModel = await _ISubjectRepo.GetSingle(x => x.Id == ids[i]);
            }

            return ResponseStatus.AddedSuccessfully;
        }
        #endregion

    }
}