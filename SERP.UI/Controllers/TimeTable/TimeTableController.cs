using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.TimeTable;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.TimeTable
{
    public class TimeTableController : Controller
    {
        private readonly IGenericRepository<TimeTableMaster, int> _ITimeTableRepository;
        private readonly IGenericRepository<BatchMaster, int> _IBatchMasterRepository;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMasterRepository;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectMasterRepository;
        private readonly IGenericRepository<AcademicMaster, int> _IAcademicMasterRepository;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _IEmployeeBasicInfoRepository;
        private readonly ITimeSheetRepo _ITimeSheetRepo;


        public TimeTableController(IGenericRepository<TimeTableMaster, int> timeTableRepo, IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<CourseMaster, int> courseMasterRepo, IGenericRepository<SubjectMaster, int> subjectRepo, IGenericRepository<AcademicMaster, int> academicMasterRepo, IGenericRepository<EmployeeBasicInfoModel, int> employeeBasicInfoRepo, ITimeSheetRepo timeSheetRepo)
        {
            _ITimeTableRepository = timeTableRepo;
            _IBatchMasterRepository = batchRepo;
            _ICourseMasterRepository = courseMasterRepo;
            _ISubjectMasterRepository = subjectRepo;
            _IAcademicMasterRepository = academicMasterRepo;
            _IEmployeeBasicInfoRepository = employeeBasicInfoRepo;
            _ITimeSheetRepo = timeSheetRepo;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CourseList = await _ICourseMasterRepository.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            ViewBag.EmployeeList = await _IEmployeeBasicInfoRepository.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/TimeTable/_TimeTableIndexPartial.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> CreateTimeTable(TimeTableMaster model)
        {
            var sessionId = HttpContext.Session.GetInt32("SessionId");
            var academicDetail = await _IAcademicMasterRepository.GetSingle(x => x.Id == sessionId);
            int totalDays = (academicDetail.EndDate - academicDetail.StartDate).Days;

            List<TimeTableMaster> models = new List<TimeTableMaster>();
            for(int i=0; i<model.Days.Count(); i++)
            {
                TimeTableMaster dataModel = new TimeTableMaster();
                dataModel.BatchId = model.BatchId;
                dataModel.CourseId = model.CourseId;
                dataModel.SubjectId = model.SubjectId;
                dataModel.EmployeeId = model.EmployeeId;
                dataModel.FromTime = model.FromTime;
                dataModel.ToTime = model.ToTime;
                dataModel.DayName = model.Days[i].ToString();
                dataModel.IsAttendClass = 0;
                dataModel.IsClassTeacher = model.IsClassTeacher;
                dataModel.SessionId = academicDetail.Id;
                models.Add(dataModel);
            }

            //for(int i=1; i<= totalDays; i++)
            //{

            //    if (model.Days.Contains(((int)academicDetail.StartDate.AddDays(i).DayOfWeek).ToString()))
            //    {
            //        TimeTableMaster dataModel = new TimeTableMaster();
            //        dataModel.BatchId = model.BatchId;
            //        dataModel.CourseId = model.CourseId;
            //        dataModel.SubjectId = model.SubjectId;
            //        dataModel.EmployeeId = model.EmployeeId;
            //        dataModel.FromTime = model.FromTime;
            //        dataModel.ToTime = model.ToTime;
            //        dataModel.TimeTableDate = academicDetail.StartDate.AddDays(i);
            //        dataModel.IsAttendClass = 0;
            //        dataModel.IsClassTeacher = model.IsClassTeacher;
            //        dataModel.SessionId = academicDetail.Id;
            //        models.Add(dataModel);
            //    }

            //}
            var response = await _ITimeTableRepository.Add(models.ToArray());
            return Json(ResponseData.Instance.GenericResponse(response));
        }

        public async Task<IActionResult> DeteTimeTable(int id)
        {
            var model = await _ITimeTableRepository.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode(model, 1);
            await _ITimeTableRepository.CreateNewContext();
            var response = await _ITimeTableRepository.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }

        public async Task<IActionResult> GetTimeTableDetails(int courseId, int batchId)
        {
            var models = await _ITimeSheetRepo.GetTimeTableModels(courseId, batchId);
            return PartialView("~/Views/TimeTable/TimeSheetDetailPartial.cshtml", models);
        }

        public async Task<IActionResult> GetTimeSheetDetails()
        {
            try
            {
                ViewBag.CourseList = await _ICourseMasterRepository.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/TimeTable/_CourseBatchTimetablePartial.cshtml");
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }
        }

        public async Task<IActionResult> CourseBatchTimeSheet(int courseId, int batchId)
        {
            var models = await _ITimeSheetRepo.GetTimeTableModels(courseId, batchId);
            TimeSheetVm timeSheetVm = new TimeSheetVm();
            List<TimeTableVm> timeTableModels = new List<TimeTableVm>();
            foreach (var data in models.GroupBy(x=>x.DayName))
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

            return PartialView("~/Views/TimeTable/_TimeTableListPartial.cshtml", timeSheetVm);
        }

        
    }
}   