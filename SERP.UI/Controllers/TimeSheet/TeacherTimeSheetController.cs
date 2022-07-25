using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LessionMaster;
using SERP.Core.Entities.TimeTable;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Controllers.TimeSheet
{
    public class TeacherTimeSheetController : Controller
    {
        private readonly IGenericRepository<TimeTableMaster, int> _ITimeTableRepository;
        private readonly IGenericRepository<BatchMaster, int> _IBatchMasterRepository;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMasterRepository;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectMasterRepository;
        private readonly IGenericRepository<LessonMaster, int> _ILessonMasterRepo;
        private readonly ITimeSheetRepo _ITimeSheetRepo;

        public TeacherTimeSheetController(IGenericRepository<TimeTableMaster, int> timeTableRepo, IGenericRepository<BatchMaster, int> batchRepo,
          IGenericRepository<CourseMaster, int> courseMasterRepo,
          IGenericRepository<SubjectMaster, int> subjectRepo,
          IGenericRepository<AcademicMaster, int> academicMasterRepo,
          IGenericRepository<LessonMaster, int> lessonRepository, ITimeSheetRepo timeSheetRepo)
        {
            _ITimeTableRepository = timeTableRepo;
            _IBatchMasterRepository = batchRepo;
            _ICourseMasterRepository = courseMasterRepo;
            _ISubjectMasterRepository = subjectRepo;
            _ILessonMasterRepo = lessonRepository;
            _ITimeSheetRepo = timeSheetRepo;
        }

        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            return View();
        }
        public async Task<IActionResult> BatchDetails(int courseId)
        {
            var response = await _IBatchMasterRepository.GetAll(x => x.CourseId == courseId && x.IsActive == 1);
            return Json(response);
        }

        public async Task<IActionResult> SubjectDetails(int courseId)
        {
            var response = await _ISubjectMasterRepository.GetAll(x => x.CourseId == courseId && x.IsActive == 1);
            return Json(response);
        }

        public async Task<IActionResult> LessonDetails(int subjectId)
        {
            var response = await _ILessonMasterRepo.GetAll(x => x.SubjectId == subjectId && x.IsActive == 1);
            return Json(response);
        }

        public async Task<IActionResult> TimeTableDetails(int subjectId, int courseId, int sectionId)
        {
            var models = (await _ITimeSheetRepo.GetTimeTableModels(courseId, sectionId)).Where(x => x.SubjectId == subjectId).ToList();
            TimeSheetVm timeSheetVm = new TimeSheetVm();
            List<TimeTableVm> timeTableModels = new List<TimeTableVm>();
            foreach (var data in models.GroupBy(x => x.DayName))
            {

                TimeTableVm timeTableVm = new TimeTableVm();
                timeTableVm.DayName = data.First().DayName;
                List<PeriodVm> periodVms = new List<PeriodVm>();
                foreach (var item in data)
                {
                    PeriodVm periodVm = new PeriodVm();
                    periodVm.CourseName = item.CourseName;
                    periodVm.BatchName = item.BatchName;
                    periodVm.FromTime = item.FromTime;
                    periodVm.ToTime = item.ToTime;
                    periodVm.EmployeeName = item.EmployeeDetail;
                    periodVm.SubjectName = item.SubjectName;

                    periodVms.Add(periodVm);
                }
                timeTableVm.PeriodModels = periodVms;
                timeTableModels.Add(timeTableVm);
            }
            timeSheetVm.TimeTableModels = timeTableModels;
            return Json(timeSheetVm);
        }

     


        #region PrivateFields
        //code to populate the default value
        private async Task PopulateViewBag()
        {
            ViewBag.ClassDetails = await _ICourseMasterRepository.GetAll(x => x.IsActive == 1 && x.IsDeleted == 0);
        }

        #endregion
    }
}
