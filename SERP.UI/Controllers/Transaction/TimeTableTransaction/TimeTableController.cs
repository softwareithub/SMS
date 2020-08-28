using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Transaction.TimeTableTransaction
{
    public class TimeTableRemoveController : Controller
    {
        private readonly IGenericRepository<BatchMaster, int> _IBatchMaster;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMaster;
        private readonly IGenericRepository<SubjectMaster, int> _subjectRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<TimeTableMasterModel, int> _timeTableMasterRepo;
        private readonly ITimeSheetRepo _timeSheetRepo;
        private readonly IGenericRepository<TimeTableAssignSubjTeacherModel, int> _timeTableAssignRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        
        public TimeTableRemoveController(IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<CourseMaster, int> courseRepo,
            IGenericRepository<SubjectMaster, int> subjectRepo,
            IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<TimeTableMasterModel, int> timeTableMasterRepo,
            IGenericRepository<TimeTableAssignSubjTeacherModel, int> timeTableAssignRepo,
            ITimeSheetRepo timeSheetRepo,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
            )
        {
            _IBatchMaster = batchRepo;
            _ICourseMaster = courseRepo;
            _subjectRepo = subjectRepo;
            _employeeRepo = employeeRepo;
            _timeTableMasterRepo = timeTableMasterRepo;
            _timeTableAssignRepo = timeTableAssignRepo;
            _timeSheetRepo = timeSheetRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Assembly a = Assembly.Load("SERP.Core.Entities");
                Type t = a.GetType(a.GetExportedTypes()[1].Name);
                ViewBag.CourseList = await _ICourseMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                ViewBag.BatchList = await _IBatchMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
                return PartialView("~/Views/TimeTable/_TimeTableIndexPartial.cshtml");
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

        public async Task<IActionResult> GetTimeTable(TimeTableMasterModel model)
        {
            try
            {
                ViewBag.SubjectList = await _subjectRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                ViewBag.EmployeeList = await _employeeRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

                var isRecordExists = (await _timeTableMasterRepo.GetList(x => x.CourseId == model.CourseId && x.BatchId == model.BatchId && x.IsActive == 1 && x.IsDeleted == 0)).Count();
                if (isRecordExists > 0)
                {
                    var result = await GetTimeSheetVm(model.CourseId, model.BatchId);
                    return PartialView("~/Views/TimeTable/_TimeTableListPartial.cshtml", result);
                }

                return CreateNewTimeSheet(model);
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
        public async Task<IActionResult> CreateTimeTable(TimeSheetVm model)
        {
            try
            {
                int courseId = Convert.ToInt32(HttpContext.Session.GetInt32("CourseId"));
                int batchId = Convert.ToInt32(HttpContext.Session.GetInt32("BatchId"));

                var isRecordExists = (await _timeTableMasterRepo.GetList(x => x.CourseId == courseId && x.BatchId == batchId
                && x.IsActive == 1 && x.IsDeleted == 0)).Count();

                if (isRecordExists > 0)
                {
                    await _timeSheetRepo.DeactivateTimeSheet(courseId, batchId);
                    await _timeTableMasterRepo.CreateNewContext();
                    await CreateTimeTable(model, courseId, batchId);
                    return Json("Time Table Updated Successfully");
                }
                else
                {
                    await _timeTableMasterRepo.CreateNewContext();
                    return await CreateTimeTable(model, courseId, batchId);
                }
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

        public async Task<IActionResult> CheckTimeSheetAvailable(int courseId, int batchId)
        {
            try
            {
                var result = await _timeTableMasterRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 &&
                         x.CourseId == courseId && x.BatchId == batchId);
                if (result.Count() > 0)
                {
                    return Json("There is already Active TimeSheet for this course and Batch Do you want to delete and Create new TimeSheet After clicking Ok your previous timesheet de-activated ?");
                }
                return Json("0");
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

        public async Task<IActionResult> DeActivateTimeSheet(int courseId, int batchId)
        {
            try
            {
                var result = await _timeSheetRepo.DeactivateTimeSheet(courseId, batchId);
                return Json("");
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

        public async Task<IActionResult> GetTimeSheetDetail(int courseId, int batchId)
        {
            try
            {
                ViewBag.SubjectList = await _subjectRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                ViewBag.EmployeeList = await _employeeRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                var result = await GetTimeSheetVm(courseId, batchId);
                HttpContext.Session.SetInt32("CourseId", courseId);
                HttpContext.Session.SetInt32("BatchId", batchId);
                return PartialView("~/Views/TimeTable/_TimeTableListPartial.cshtml", result);
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

        public async Task<IActionResult> GetTimeSheet(int courseId, int batchId)
        {
            try
            {
                var result = await _timeSheetRepo.GetTimeSheetDetailsByCourseIdBatchId(courseId, batchId);
                return PartialView("~/Views/TimeTable/_CourseBatchTimeSheetPartial.cshtml", result);
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

        public async Task<IActionResult> GetTimeSheetDetails()
        {
            try
            {
                ViewBag.CourseList = await _ICourseMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/TimeTable/TimeSheetDetailPartial.cshtml");
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

        public async Task<IActionResult> AssignEmployee(int timeTableId, string fromTime, string toTime)
        {
            try
            {
                HttpContext.Session.SetInt32("TimeTableId", timeTableId);
                TimeSpan.TryParse(fromTime, out TimeSpan outFromTime);
                TimeSpan.TryParse(toTime, out TimeSpan outToTime);
                var response = await _timeSheetRepo.AssignTeacherTemp(outFromTime, outToTime);
                return PartialView("~/Views/TimeTable/_AssignEmployee.cshtml", response);
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
        private IActionResult CreateNewTimeSheet(TimeTableMasterModel model)
        {
            List<string> days = new List<string>();
            model.Days.ToList().ForEach(item =>
            {
                days.Add(((DayOfWeek)item).ToString());
            });


            List<string> periods = new List<string>();
            for (int i = 1; i <= model.Periods; i++)
            {
                periods.Add($"Period{i}");
            }

            TimeSheetVm timeSheetVm = new TimeSheetVm();
            List<TimeTableVm> timeTableVms = new List<TimeTableVm>();


            for (int i = 0; i < days.Count(); i++)
            {
                TimeTableVm timeTableModel = new TimeTableVm();
                timeTableModel.DayName = days[i];
                List<PeriodVm> periodVms = new List<PeriodVm>();
                for (int k = 0; k < periods.Count(); k++)
                {
                    PeriodVm periodModel = new PeriodVm();
                    //periodModel.Period = periods[k];
                    periodVms.Add(periodModel);
                }
                timeTableModel.PeriodModels = periodVms;
                timeTableVms.Add(timeTableModel);
            }

            timeSheetVm.TimeTableModels = timeTableVms;


            //TempData["CourseId"] 
            HttpContext.Session.SetInt32 ("CourseId", model.CourseId);
            HttpContext.Session.SetInt32("BatchId", model.BatchId);
            TempData["BatchId"] = model.BatchId;
            TempData["Days"] = string.Join(",", model.Days);
            TempData["PeriodName"] = model.Periods;

            return PartialView("~/Views/TimeTable/_TimeTableListPartial.cshtml", timeSheetVm);
        }

        private async Task<TimeSheetVm> GetTimeSheetVm(int courseId, int batchId)
        {

            return await _timeSheetRepo.GetAllIncludeTimeSheetDetails(courseId, batchId);
        }

        private async Task<IActionResult> CreateTimeTable(TimeSheetVm model, int courseId, int batchId)
        {
            try
            {
                List<TimeTableMasterModel> masterModels = new List<TimeTableMasterModel>();

                model.TimeTableModels.ForEach(item =>
                {
                    TimeTableMasterModel masterModel = new TimeTableMasterModel();
                    masterModel.BatchId = batchId;
                    masterModel.CourseId = courseId;
                    masterModel.Name = item.DayName;

                    List<TimeTableAssignSubjTeacherModel> assignSubjectModels = new List<TimeTableAssignSubjTeacherModel>();

                    item.PeriodModels.ForEach(data =>
                    {
                        TimeTableAssignSubjTeacherModel assignModel = new TimeTableAssignSubjTeacherModel();
                            //assignModel.PeriodName = data.Period;
                            //assignModel.SubjecId = data.SubjectId;
                        assignModel.TeacherId = data.EmployeeId;
                        assignModel.FromTime = data.FromTime;
                        assignModel.ToTime = data.ToTime;

                        assignSubjectModels.Add(assignModel);
                    });

                    masterModel.TimeTableAssignSubjTeacherModels = assignSubjectModels;
                    masterModels.Add(masterModel);

                });

                var result = await _timeTableMasterRepo.Add(masterModels.ToArray());
                return Json("Time Table Created");
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
        #endregion
    }
}