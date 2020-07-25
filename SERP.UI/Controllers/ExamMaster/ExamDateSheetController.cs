using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Model.ExamModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.ExamMaster
{

    public class ExamDateSheetController : Controller
    {
        private readonly IGenericRepository<BatchMaster, int> _IBatchMaster;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMaster;
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _basicInfoRepo;
        private readonly IGenericRepository<Exam, int> _examRepo;
        private readonly IGenericRepository<ExamSheet, int> _IExamSheetRepo;


        public ExamDateSheetController(IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<CourseMaster, int> courseRepo,
            ISubjectMasterRepo subjectRepo,
            IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<Exam, int> examRepo, IGenericRepository<ExamSheet, int> examSheetRepo)
        {
            _IBatchMaster = batchRepo;
            _ICourseMaster = courseRepo;
            _ISubjectRepo = subjectRepo;
            _basicInfoRepo = employeeRepo;
            _examRepo = examRepo;
            _IExamSheetRepo = examSheetRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.ExamList = await _examRepo.GetList(x => x.IsActive == 1);
            ViewBag.CourseList = await _ICourseMaster.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/ExamMaster/ExamDateSheetPartial.cshtml");
        }

        public async Task<IActionResult> GetTimeSheet(int examId,int courseId)
        {
            var result = await GetExamList(examId,courseId);
            return PartialView("~/Views/ExamMaster/_ExamWiseDateSheetPartial.cshtml", result);
        }

        private async Task<List<ExamSheetVm>> GetExamList(int examId,int courseId)
        {
            var result = (from ES in await _IExamSheetRepo.GetList(x => x.IsActive == 1 && x.ExamId==examId && x.CourseId==courseId)
                          join EM in await _examRepo.GetList(x => x.IsActive == 1)
                          on ES.ExamId equals EM.Id
                          join CM in await _ICourseMaster.GetList(x => x.IsActive == 1)
                          on ES.CourseId equals CM.Id
                          join BM in await _IBatchMaster.GetList(x => x.IsActive == 1)
                          on ES.BatchId equals BM.Id
                          join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                          on ES.SubjectId equals SM.Id
                          join EMP in await _basicInfoRepo.GetList(x => x.IsActive == 1)
                          on ES.InveligitatorId equals EMP.Id
                          select new ExamSheetVm
                          {
                              ExamId = ES.Id,
                              ExamName = EM.ExamName,
                              CourseName = CM.Name,
                              BatchName = BM.BatchName,
                              SubjectName = SM.SubjectName,
                              SubjectCode=SM.SubjectCode,
                              ExamDate = ES.ExamDate,
                              StartTime = ES.StartTime,
                              EndTime = ES.EndTime,
                              TeacherName = EMP.Name,
                              MaxMark = ES.MaxMark,
                              PassMark = ES.PassMark
                          }).ToList();

            return result;
        }
    }
}