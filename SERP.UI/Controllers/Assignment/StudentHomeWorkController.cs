using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.HomeAssignment;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.AssignmentHomeModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Assignment
{
    public class StudentHomeWorkController : Controller
    {
        private readonly IGenericRepository<HomeWorkModel, int> _IHomeWorkRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _IEmployeeRepo;
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;
        private readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<StudentHomeWork, int> _IStudentHomeWorkRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        

        public  StudentHomeWorkController(IGenericRepository<HomeWorkModel, int> homeworkRepo,
            IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<SubjectMaster, int> subjectRepo,
            IGenericRepository<EmployeeBasicInfoModel, int> employeRepo,
            IGenericRepository<StudentMaster, int> studentRepo,
             IGenericRepository<StudentPromote, int> studentPromte,
            IGenericRepository<StudentHomeWork, int>  studentHomeWorkRepo,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
            )
        {
            _IHomeWorkRepo = homeworkRepo;
            _ICourseRepo = courseRepo;
            _IBatchRepo = batchRepo;
            _ISubjectRepo = subjectRepo;
            _IEmployeeRepo = employeRepo;
            _IStudentMaster = studentRepo;
            _IStudentPromote = studentPromte;
            _IStudentHomeWorkRepo = studentHomeWorkRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/AssignmentWork/_HomeWorkSubmissionPartial.cshtml");
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

        public async Task<IActionResult> GetHomeWorkDetail(int courseId, int batchId, int subjectId)
        {
            try
            {
                var result = (from HM in await _IHomeWorkRepo.GetList(x => x.IsActive == 1)
                              join CM in await _ICourseRepo.GetList(x => x.IsActive == 1 && x.Id == courseId)
                              on HM.CourseId equals CM.Id
                              join BM in await _IBatchRepo.GetList(x => x.IsActive == 1 && x.Id == batchId)
                              on HM.BatchId equals BM.Id
                              join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.Id == subjectId)
                              on HM.SubjectId equals SM.Id
                              join EM in await _IEmployeeRepo.GetList(x => x.IsActive == 1)
                              on HM.EmployeeId equals EM.Id
                              select new HomeWorkModelVm
                              {
                                  Id = HM.Id,
                                  CourseName = CM.Name,
                                  BatchName = BM.BatchName,
                                  SubjectName = SM.SubjectName,
                                  EmployeeName = EM.Name,
                                  HomeWork = HM.HomeWork,
                                  PDFPath = HM.PDFPath,
                                  PublishDate = Convert.ToDateTime(HM.HomeWorkDate),
                                  SubmissionDate = Convert.ToDateTime(HM.HomeWorkSubmissionDate),
                              }).ToList();

                HttpContext.Session.SetInt32("courseId", courseId);
                HttpContext.Session.SetInt32("batchId", batchId);
                HttpContext.Session.SetInt32("subjectId", subjectId);

                return PartialView("~/Views/AssignmentWork/_HomeWorkDetailForCheck.cshtml", result);
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

        public async Task<IActionResult> GetStudentList(int homeWorkId)
        {
            try
            {
                int courseId = Convert.ToInt32(HttpContext.Session.GetInt32("courseId"));
                int batchId = Convert.ToInt32(HttpContext.Session.GetInt32("batchId"));
                var result = (from SM in await _IStudentMaster.GetList(x => x.IsActive == 1)
                              join SP in await _IStudentPromote.GetList(x => x.IsActive == 1 &&
                                                             x.CourseId == courseId && x.BatchId == batchId)
                              on SM.Id equals SP.StudentId
                              select new HomeWorkAllocationModel
                              {
                                  StudentId = SM.Id,
                                  StudentName = SM.Name,
                                  RollCode = SM.RollCode,
                                  ImagePath = SM.StudentPhoto,
                              }).ToList();
                var submittedHomeWork = await _IStudentHomeWorkRepo.GetList(x => x.HomeWorkId == homeWorkId && x.IsActive == 1);
                if (submittedHomeWork.Count() > 0)
                {
                    result.ForEach(item =>
                    {
                        var subMittedModel = submittedHomeWork.ToList().Find(x => x.StudentId == item.StudentId);
                        if (subMittedModel != null)
                        {
                            item.Grade = subMittedModel.Grade;
                            item.FeedBack = subMittedModel.Reason;
                            item.SubMissionDate = subMittedModel.SubmissionDate;
                        }

                    });
                }
                HttpContext.Session.SetInt32("homeWorkId", homeWorkId);
                return PartialView("~/Views/AssignmentWork/StudentHomeWorkGradeAllocation.cshtml", result);
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
        public async Task<IActionResult> PostStudentMark(string submissionDate,string grades,string complains, string studentIds)
        {
            try
            {
                List<StudentHomeWork> models = new List<StudentHomeWork>();
                int homeWorkId = Convert.ToInt32(HttpContext.Session.GetInt32("homeWorkId"));
                for (int i = 0; i < studentIds.Split(',').Count(); i++)
                {
                    StudentHomeWork model = new StudentHomeWork();
                    model.StudentId = Convert.ToInt32(studentIds.Split(',')[i]);
                    model.Grade = grades.Split(',')[i];
                    model.Reason = complains.Split(',')[i];
                    model.SubmissionDate = Convert.ToDateTime(string.IsNullOrEmpty(submissionDate.Split(',')[i]) ? DateTime.Now.ToString() : submissionDate.Split(',')[i]);
                    model.HomeWorkId = homeWorkId;
                    model.IsActive = 1;
                    model.CreatedBy = 1;
                    model.CreatedDate = DateTime.Now.Date;
                    models.Add(model);
                }
                await InActivePreviousData(homeWorkId);
                var response = await _IStudentHomeWorkRepo.Add(models.ToArray());
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }

        private async Task InActivePreviousData(int homeWorkId)
        {
            await _IStudentHomeWorkRepo.CreateNewContext();
            var result = await _IStudentHomeWorkRepo.GetList(x => x.HomeWorkId == homeWorkId);
            if(result.Count()>0)
            {
                result.ToList().ForEach(item => {
                    item.IsActive = 0;
                    item.IsDeleted = 1;
                });
                await _IStudentHomeWorkRepo.CreateNewContext();
                var response = await _IStudentHomeWorkRepo.Update(result.ToArray());
                await _IStudentHomeWorkRepo.CreateNewContext();
            }

        }


        public async Task<IActionResult> GetBatchList(int id)
        {
            try
            {
                var data = await _IBatchRepo.GetList(z => z.CourseId == id);
                return Json(data);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }
        public async Task<IActionResult> GetSubjectList(int id)
        {
            try
            {
                var data = await _ISubjectRepo.GetList(z => z.CourseId == id);
                return Json(data);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }

        public async Task<IActionResult> GetTeachers(int courseId)
        {
            try
            {
                var models = await _IEmployeeRepo.GetList(x => x.IsActive == 1);
                return Json(models);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }
    }
}