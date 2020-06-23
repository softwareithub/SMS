using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LessionMaster;
using SERP.Core.Model.AssignmentHomeModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Assignment
{
    public class LessonMasterController : Controller
    {
        private readonly IGenericRepository<LessonMaster, int> _ILessonMasterRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMasterRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        public LessonMasterController(IGenericRepository<LessonMaster, int> lessonMasterRepo,
                IGenericRepository<CourseMaster, int> courseMasterRepo, IGenericRepository<SubjectMaster, int> subjectRepo)
        {
            _ILessonMasterRepo = lessonMasterRepo;
            _ICourseMasterRepo = courseMasterRepo;
            _ISubjectRepo = subjectRepo;
        }
        public async Task<IActionResult> CreateLesson(int id)
        {
            var model = await _ILessonMasterRepo.GetSingle(x => x.Id == id);
            ViewBag.ClassList = await _ICourseMasterRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/AssignmentWork/_LessonMasterPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson(LessonMaster model)
        {
            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now;
                var response = await _ILessonMasterRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var response = await _ILessonMasterRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }

        public async Task<IActionResult> GetLessonPlanning()
        {
            var result = (from LM in await _ILessonMasterRepo.GetList(x => x.IsActive == 1)
                          join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                          on LM.SubjectId equals SM.Id
                          select new LessonPlanningModel
                          {
                              LessonId = LM.Id,
                              LessonMasterModel = new LessonMaster()
                              {
                                  LessonName= LM.LessonName,
                                  StartDate= LM.StartDate,
                                  EndDate= LM.EndDate,
                                  ClassTestDate= LM.ClassTestDate
                              },
                              SubjectName= SM.SubjectName

                          }).ToList();

            return PartialView("~/Views/AssignmentWork/_LessionDetailPartial.cshtml", result);
        }

        public async Task<IActionResult> GetSubjectList(int courseId)
        {
            return Json(await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.CourseId == courseId));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _ILessonMasterRepo.GetSingle(x => x.Id == id);
            model.IsActive = 0;
            model.IsDeleted = 1;
            await _ILessonMasterRepo.CreateNewContext();
            var response = await _ILessonMasterRepo.Update(model);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}