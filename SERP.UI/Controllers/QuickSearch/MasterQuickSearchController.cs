using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.ExamModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Core.Entities.Entity.Core.HRModule;
using System.Linq.Expressions;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Utilities.ExceptionHelper;

namespace SERP.UI.Controllers.QuickSearch
{
    public class MasterQuickSearchController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IQuickSearchRepo _QuickSearchRepo;
        private readonly IGenericRepository<GuardianMaster, int> _guardianRepo;
        private readonly IGenericRepository<StudentEducationalDetail, int> _studentEducationReport;
        private readonly IGenericRepository<StudentMarkAllocation, int> _IStudentMarkRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<ExamSheet, int> _IExamSheetRepo;
        private readonly IGenericRepository<GradeMaster, int> _IGradeRepo;
        private readonly IGenericRepository<Exam, int> _IExamRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public MasterQuickSearchController(IGenericRepository<StudentMaster, int> studentRepo,
            IQuickSearchRepo quickSearchRepo, IGenericRepository<GuardianMaster, int> guardianRepo, IGenericRepository<StudentEducationalDetail, int> studentEducationalReport, IGenericRepository<StudentMarkAllocation, int> studentMarkAllocation, IGenericRepository<SubjectMaster, int> subjectRepo, IGenericRepository<ExamSheet, int> examSheetRepo, IGenericRepository<GradeMaster, int> gradeMasterRepo, IGenericRepository<Exam, int> examRepo, IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _studentRepo = studentRepo;
            _QuickSearchRepo = quickSearchRepo;
            _guardianRepo = guardianRepo;
            _studentEducationReport = studentEducationalReport;
            _IStudentMarkRepo = studentMarkAllocation;
            _ISubjectRepo = subjectRepo;
            _IExamSheetRepo = examSheetRepo;
            _IGradeRepo = gradeMasterRepo;
            _IExamRepo = examRepo;
            _employeeRepo = employeeRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(string studentName)
        {
            try
            {
                studentName = studentName.Split("(")[0];
                var studentId = (await _studentRepo.GetSingle(x => x.Name.ToLower().Trim() == studentName.ToLower().Trim() && x.IsActive==1 )).Id;
                HttpContext.Session.SetInt32("StudentId", studentId);
                return await Task.Run(() => PartialView("~/Views/QuickMasterSearch/_QuickStudentSearchPartial.cshtml"));
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("Index", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> StudentBasicInfo()
        {
            try
            {
                ViewBag.HeaderDetail = "Student Basic Information";
                var model = await _QuickSearchRepo.GetStudentBasicInfo(Convert.ToInt32(HttpContext.Session.GetInt32("StudentId")));
                return PartialView("~/Views/QuickMasterSearch/_StudentBasicInfoPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("StudentBasicInfo", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }
        }

        public async Task<IActionResult> GetStudentAttandenceReport(int monthId, int yearId)
        {
            try
            {
                ViewBag.HeaderDetail = "Student Attandence Report";
                var models = await _QuickSearchRepo.GetStudentAttandenceReport(Convert.ToInt32(HttpContext.Session.GetInt32("StudentId")), monthId, yearId);
                return PartialView("~/Views/QuickMasterSearch/_StudentAttandenceReportPartial.cshtml", models);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("GetStudentAttandenceReport", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> GuardinaListPartial()
        {
            try
            {
                ViewBag.HeaderDetail = "Student Guardian List";
                var guardianDetails = await _guardianRepo.GetList(x => x.IsActive == 1 && x.StudentId == Convert.ToInt32(HttpContext.Session.GetInt32("StudentId")));
                return PartialView("~/Views/QuickMasterSearch/_StudentGuardianInfoPartial.cshtml", guardianDetails);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("GuardinaListPartial", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }
        }

        public async Task<IActionResult> EducationalDetail()
        {
            try
            {
                ViewBag.HeaderDetail = "Student Educational Detail";
                var educationalDetails = await _studentEducationReport.GetList(x => x.IsActive == 1 && x.StudentId == Convert.ToInt32(HttpContext.Session.GetInt32("StudentId")));

                return PartialView("~/Views/QuickMasterSearch/_EducationDetailPartial.cshtml", educationalDetails);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("EducationalDetail", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> StudentFeeDetail()
        {
            try
            {
                ViewBag.HeaderDetail = "Student Fee Detail";
                var model = await _QuickSearchRepo.StudentFeeDetailReport(Convert.ToInt32(HttpContext.Session.GetInt32("StudentId")));
                return PartialView("~/Views/QuickMasterSearch/_StudentFeeDetailPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("StudentFeeDetail", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> ExamDetails()
        {
            try
            {
                ViewBag.HeaderDetail = "Student Exam Details Detail";
                ViewBag.ExamList = await _IExamRepo.GetList(x => x.IsActive == 1);
                return await Task.Run(() => PartialView("~/Views/QuickMasterSearch/_StudentExamDetailPartial.cshtml"));
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("ExamDetails", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }
        public async Task<IActionResult> StudentExam(int examId)
        {
            try
            {
                var gradeList = await _IGradeRepo.GetList(x => x.IsActive == 1);
                int studentId = Convert.ToInt32(HttpContext.Session.GetInt32("StudentId"));
                var markAllocationDetails = (from EM in await _IStudentMarkRepo.GetList(x => x.IsActive == 1
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
                                                 Grade = GradeCalcullation(Convert.ToInt32(EM.AssignedMark + EM.LabMarks), gradeList.ToList())

                                             }).Distinct().ToList();
                return PartialView("~/Views/QuickMasterSearch/_StudentExamReportPartial.cshtml", markAllocationDetails);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("StudentExam", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }



        public async Task<IActionResult> BookDetails()
        {
            try
            {
                ViewBag.HeaderDetail = "Book Details";
                var model = await _QuickSearchRepo.GetBookDetails();
                return PartialView("~/Views/QuickMasterSearch/_BookDetailPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("BookDetails", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> EmployeeQuickSearch(string employee)
        {
            try
            {
                employee = employee.Split("(")[0];
                var model = await _employeeRepo.GetSingle(x => x.Name.ToLower().Trim() == employee.Trim().ToLower());
                var employeeDetail = await _QuickSearchRepo.GetEmployeeInformationModel(model.Id);
                HttpContext.Session.SetInt32("employeeId", model.Id);
                return PartialView("~/Views/QuickMasterSearch/_EmployeeQuickSearchPartial.cshtml", employeeDetail);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("EmployeeQuickSearch", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> GetAbsentTeacher()
        {
            try
            {
                var model = await _QuickSearchRepo.GetAbsentEmployeeModels();
                return PartialView("~/Views/QuickMasterSearch/_AbsentTeacherDetailPartial.cshtml", model);
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("GetAbsentTeacher", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> GetClassAllocation()
        {
            try
            {
                var model = await _QuickSearchRepo.GetEmployeeClassAllocation(Convert.ToInt32(HttpContext.Session.GetInt32("employeeId")));
                return PartialView("~/Views/QuickMasterSearch/_EmployeeClassAllocationPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("GetAbsentTeacher", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }
        }

        public async Task<IActionResult> GetEmployeeReview()
        {
            try
            {
                return PartialView("~/Views/QuickMasterSearch/_ParentReviewPartial.cshtml");
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("GetAbsentTeacher", "MasterQuickSearch", ex.Message, "HTTPGETCALL", 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }
        }
        #region PrivateReagion

        private string GradeCalcullation(int marks, List<GradeMaster> gradeMasters)
        {
            string grade = string.Empty;

            gradeMasters.ToList().ForEach(item =>
            {
                if (Convert.ToInt32(item.FromMarks) >= marks && marks <= Convert.ToInt32(item.ToMarks))
                {
                    grade = item.GradeName;
                }
            });
            return grade;
        }
        #endregion
    }
}