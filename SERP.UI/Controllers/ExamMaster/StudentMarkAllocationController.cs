using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.ExamModel;
using SERP.Core.Model.MasterViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using static System.Convert;

namespace SERP.UI.Controllers.ExamMaster
{
    public class StudentMarkAllocationController : Controller
    {
        private readonly IGenericRepository<StudentMarkAllocation, int> _IStudentMarkRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<ExamSheet, int> _IExamSheetRepo;
        private readonly IGenericRepository<SERP.Core.Entities.Entity.Core.ExamDetail.Exam, int> _IExamRepo;
        private readonly IGenericRepository<GradeMaster, int> _IGradeRepo;
        private readonly IGenericRepository<InstituteMaster, int> _IInstituteRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public StudentMarkAllocationController(IGenericRepository<StudentMarkAllocation, int> studentMarkRepo,
            IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<StudentMaster, int> studentRepo,
            IGenericRepository<StudentPromote, int> studentPromoteRepo, IGenericRepository<SubjectMaster, int> subjectRepo,
            IGenericRepository<SERP.Core.Entities.Entity.Core.ExamDetail.Exam, int> _examRepo,
            IGenericRepository<ExamSheet, int> examSheetRepo, IGenericRepository<GradeMaster, int> gradeRepo,
            IGenericRepository<InstituteMaster, int> instituteRepo, IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
            )
        {
            _IStudentMarkRepo = studentMarkRepo;
            _ICourseRepo = courseRepo;
            _IStudentMaster = studentRepo;
            _IStudentPromote = studentPromoteRepo;
            _ISubjectRepo = subjectRepo;
            _IExamRepo = _examRepo;
            _IExamSheetRepo = examSheetRepo;
            _IGradeRepo = gradeRepo;
            _IInstituteRepo = instituteRepo;
            _IBatchRepo = batchRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;            
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                ViewBag.SubjectList = await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                ViewBag.ExamList = await _IExamRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/ExamMaster/_StudentMarkAllocation.cshtml");
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

        public async Task<IActionResult> GetSubjectDetail(int courseId)
        {
            var result = await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.CourseId == courseId);
            return Json(result);
        }

        public async Task<IActionResult> GetStudentList(int courseId, int batchId, int examId, int subjectId)
        {
            try
            {
                List<StudentMarkAllocationVm> result = await StudentDetailForMarkAllocation(courseId, batchId, examId, subjectId);
                HttpContext.Session.SetInt32("courseId", courseId);
                HttpContext.Session.SetInt32("batchId", batchId);
                HttpContext.Session.SetInt32("examId", examId);
                HttpContext.Session.SetInt32("subjectId", subjectId);

                return PartialView("~/Views/ExamMaster/_MarkAllocationPartial.cshtml", result);
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
        public async Task<IActionResult> AllocateStudentMark()
        {
            var theoryNumbers = Request.Form["Theory"].ToString().Split(',');
            var labNumbers = Request.Form["lab"].ToString().Split(',');
            var studentIds = Request.Form["studentId"].ToString().Split(',');

            var previousModels = await _IStudentMarkRepo.GetList(x => x.ExamId == ToInt32(HttpContext.Session.GetInt32("examId"))
             && x.SubjectId == ToInt32(HttpContext.Session.GetInt32("subjectId"))
            );
            previousModels.ToList().ForEach(item =>
            {
                item.IsActive = 0;
                item.IsDeleted = 1;
            });

            var updateExamSheet = await _IStudentMarkRepo.Update(previousModels.ToArray());

            await _IStudentMarkRepo.CreateNewContext();

            List<StudentMarkAllocation> models = new List<StudentMarkAllocation>();
            for (int i = 0; i < studentIds.Count(); i++)
            {
                StudentMarkAllocation model = new StudentMarkAllocation();
                model.StudentId = ToInt32(studentIds[i]);
                model.AssignedMark = ToDecimal(theoryNumbers[i]);
                model.LabMarks = ToDecimal(labNumbers[i]);
                model.IsActive = 1;
                model.CreatedBy = 1;
                model.ExamId = Convert.ToInt32(HttpContext.Session.GetInt32("examId"));
                model.SubjectId = ToInt32(HttpContext.Session.GetInt32("subjectId"));
                models.Add(model);
            }

            var result = await _IStudentMarkRepo.Add(models.ToArray());
            return Json(ResponseData.Instance.GenericResponse(result));
        }
        public async Task<IActionResult> GetGrade(int marks)
        {
            var gradeList = await _IGradeRepo.GetList(x => x.IsActive == 1);
            string grade =  GradeCalcullation(marks, gradeList.ToList());
            return Json(grade);
        }


        public async Task<IActionResult> StudentMarkSheetSearch()
        {
            try
            {
                ViewBag.ExamList = await _IExamRepo.GetList(x => x.IsActive == 1);
                ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/ExamMaster/_studentMarkSheetSearchPartial.cshtml");
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
        public async Task<IActionResult> GetStudentSearcList(int courseId, int batchId)
        {
            List<StudentPartialInfoViewModel> models = new List<StudentPartialInfoViewModel>();
            models = (from SP in await _IStudentPromote.GetList(x => x.IsActive == 1 && x.CourseId==courseId && x.BatchId== batchId)
                      join SM in await _IStudentMaster.GetList(x => x.IsActive == 1)
                      on SP.StudentId equals SM.Id
                      join CM in await _ICourseRepo.GetList(x => x.IsActive == 1)
                      on SP.CourseId equals CM.Id
                      join BM in await _IBatchRepo.GetList(x => x.IsActive == 1)
                      on SP.BatchId equals BM.Id

                      select new StudentPartialInfoViewModel
                      {
                          Id = SP.StudentId,
                          RollCode = SM.RollCode,
                          Registration = SM.RegistrationNumber,
                          CourseName = CM.Name,
                          BatchName = BM.BatchName,
                          StudentName = SM.Name,
                          FatherName = SM.FatherName,
                          MotherName = SM.MotherName,
                          StudentPhoto = SM.StudentPhoto,
                          DateOfBirth = SM.DateOfBirth,
                          Gender = SM.Gender,
                          JoiningDate = SM.JoiningDate
                      }).ToList();

            return Json(models);
        }

        public async Task<IActionResult> StudentMarkSheet(int studentId, int examId)
        {
            try
            {

                StudentMarkSheetVm model = new StudentMarkSheetVm();
                var gradeList = await _IGradeRepo.GetList(x => x.IsActive == 1);
                model.InstituteModel = await _IInstituteRepo.GetSingle(x => x.IsActive == 1);

                model.StudentInfoModel = (from SP in await _IStudentPromote.GetList(x => x.IsActive == 1)
                                          join SM in await _IStudentMaster.GetList(x => x.IsActive == 1)
                                          on SP.StudentId equals SM.Id
                                          join CM in await _ICourseRepo.GetList(x => x.IsActive == 1)
                                          on SP.CourseId equals CM.Id
                                          join BM in await _IBatchRepo.GetList(x => x.IsActive == 1)
                                          on SP.BatchId equals BM.Id

                                          select new StudentPartialInfoViewModel
                                          {
                                              Id = SP.StudentId,
                                              RollCode = SM.RollCode,
                                              Registration = SM.RegistrationNumber,
                                              CourseName = CM.Name,
                                              BatchName = BM.BatchName,
                                              StudentName = SM.Name,
                                              FatherName = SM.FatherName,
                                              MotherName = SM.MotherName,
                                              StudentPhoto = SM.StudentPhoto,
                                              DateOfBirth = SM.DateOfBirth,
                                              Gender = SM.Gender,
                                              JoiningDate = SM.JoiningDate
                                          }).ToList().Where(x => x.Id == studentId).FirstOrDefault();

                model.ExamModel = await _IExamRepo.GetSingle(x => x.Id == examId);

                model.MarkAllocationVms = (from EM in await _IStudentMarkRepo.GetList(x => x.IsActive == 1
                                                                      && x.ExamId == examId && x.StudentId == studentId)
                                           join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                                           on EM.SubjectId equals SM.Id
                                           join ES in await _IExamSheetRepo.GetList(x => x.IsActive == 1 && x.ExamId == examId)
                                           on EM.ExamId equals ES.ExamId
                                           select new StudentMarkAllocationVm
                                           {
                                               SubjectName = SM.SubjectName,
                                               PassMark = ES.PassMark,
                                               MaxMarks = ES.MaxMark,
                                               AssignedMarks = EM.AssignedMark,
                                               LabMarks = EM.LabMarks,
                                               Percentage = ((EM.AssignedMark + EM.LabMarks) / ES.MaxMark) * 100,
                                               Grade = GradeCalcullation(ToInt32(EM.AssignedMark + EM.LabMarks), gradeList.ToList())

                                           }).ToList();

                return PartialView("~/Views/ExamMaster/_StudentMarkSheet.cshtml", model);
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
        #region PrivateFields
        private async Task<List<StudentMarkAllocationVm>> StudentDetailForMarkAllocation(int courseId, int batchId, int examId, int subjectId)
        {
            var studentPromotes = await _IStudentPromote.GetList(x => x.IsActive == 1 && x.CourseId == courseId && x.BatchId == batchId);
            var students = await _IStudentMaster.GetList(x => x.IsActive == 1);
            var examDetail = await _IExamSheetRepo.GetSingle(x => x.CourseId == courseId && x.BatchId == batchId
            && x.SubjectId == subjectId && x.IsActive == 1);

            var result = (from SP in studentPromotes
                          join SM in students
                          on SP.StudentId equals SM.Id
                          select new StudentMarkAllocationVm
                          {
                              StudentId = SP.StudentId,
                              RollCode = SM.RollCode,
                              StudentName = SM.Name,
                              PassMark = examDetail.PassMark,
                              MaxMarks = examDetail.MaxMark
                          }).ToList();
            var studentMarks = await _IStudentMarkRepo.GetList(x => x.ExamId == examId && x.IsActive == 1 && x.SubjectId == subjectId);
            studentMarks.ToList().ForEach(x =>
            {
                StudentMarkAllocationVm studentModel = result.Where(i => i.StudentId == x.StudentId).First();
                studentModel.AssignedMarks = x.AssignedMark;
                studentModel.LabMarks = x.LabMarks;
                studentModel.Percentage = ((studentModel.AssignedMarks + studentModel.LabMarks) / studentModel.MaxMarks) * 100;
            });
            return result;
        }

        private  string GradeCalcullation(int marks,List<GradeMaster> gradeMasters)
        {
            string grade = string.Empty;
           
            gradeMasters.ToList().ForEach(item =>
            {
                if (ToInt32(item.FromMarks) >= marks && marks<= ToInt32(item.ToMarks))
                {
                    grade = item.GradeName;
                }
            });
            return grade;
        }

        
        #endregion
    }
}