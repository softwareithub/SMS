using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.ExamModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.ExamMaster
{
    public class ExamSheetController : Controller
    {
        private readonly IGenericRepository<BatchMaster, int> _IBatchMaster;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMaster;
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _basicInfoRepo;
        private readonly IGenericRepository<Exam, int> _examRepo;
        private readonly IGenericRepository<ExamSheet, int> _IExamSheetRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        

        public ExamSheetController(IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<CourseMaster, int> courseRepo,
            ISubjectMasterRepo subjectRepo,
            IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<Exam, int> examRepo, IGenericRepository<ExamSheet, int> examSheetRepo,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IBatchMaster = batchRepo;
            _ICourseMaster = courseRepo;
            _ISubjectRepo = subjectRepo;
            _basicInfoRepo = employeeRepo;
            _examRepo = examRepo;
            _IExamSheetRepo = examSheetRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                await PopulateViewBag();
                var result = await _IExamSheetRepo.GetSingle(x => x.Id == id);
                return View("~/Views/ExamMaster/_ExamTimeSheet.cshtml", result);
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

        public async Task<IActionResult> CreateTimeSheet(ExamSheet model)
        {
            if (model.Id == 0)
            {
                var result = await _IExamSheetRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                var result = await _IExamSheetRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }

        }

        public async Task<IActionResult> GetTimeSheet()
        {
            try
            {
                var result = await GetExamList();
                return PartialView("~/Views/ExamMaster/_ExamSheetPartial.cshtml", result);
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

        private async Task PopulateViewBag()
        {
            ViewBag.CourseList = await _ICourseMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            ViewBag.BatchList = await _IBatchMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            ViewBag.EmployeeList = await _basicInfoRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            var examResult = await _examRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            ViewBag.ExamList = examResult;

        }

        public async Task<IActionResult> GetSubjectJson()
        {
            var result = await _ISubjectRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            return Json(result);
        }
        public async Task<IActionResult> GetDateTimeList(int id)
        {
            List<DateTime> allDates = new List<DateTime>();
            if (id != 0)
            {
                var model = await _examRepo.GetSingle(x => x.IsActive == 1 && x.IsDeleted == 0 && x.Id == id);
                for (DateTime date = model.StartDate; date <= model.EndDate; date = date.AddDays(1))
                    allDates.Add(date);
            }
            return Json(allDates); ;
        }

        public async Task<IActionResult> ExamTimeSheet()
        {
            return Json(await GetExamList());
        }

        private async Task<List<ExamSheetVm>> GetExamList()
        {
            var result = (from ES in await _IExamSheetRepo.GetList(x => x.IsActive == 1)
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
                              ExamDate = ES.ExamDate,
                              StartTime = ES.StartTime,
                              EndTime = ES.EndTime,
                              TeacherName = EMP.Name,
                              MaxMark = ES.MaxMark,
                              PassMark = ES.PassMark
                          }).ToList();

            return result;
        }

        public async Task<IActionResult> DeleteExamSheet(int id)
        {
            var model = await _IExamSheetRepo.GetSingle(x => x.Id == id);
            await _IExamSheetRepo.CreateNewContext();
            model.IsActive = 0;
            model.IsDeleted = 1;
            var result = await _IExamSheetRepo.Update(model);
            return Json(ResponseData.Instance.GenericResponse(result));
        }
    }
}