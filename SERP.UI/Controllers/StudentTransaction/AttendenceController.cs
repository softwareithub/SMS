using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.StudentTransaction;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Extension;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.StudentTransaction
{
    public class AttendenceController : Controller
    {
        private readonly ITimeSheetRepo _timeSheetRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        public readonly IGenericRepository<StudentAttendenceModel, int> _IStudentAttendence;

        public AttendenceController(ITimeSheetRepo timeSheetRepo,
            IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<StudentMaster, int> studentRepo,
            IGenericRepository<StudentPromote, int> studentPromoteRepo, IGenericRepository<StudentAttendenceModel, int> _studentAttendenceRepo)
        {
            _timeSheetRepo = timeSheetRepo;
            _ICourseRepo = courseRepo;
            _IStudentMaster = studentRepo;
            _IStudentPromote = studentPromoteRepo;
            _IStudentAttendence = _studentAttendenceRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return await Task.Run(() => PartialView("~/Views/StudentAttendence/_CreateStudentAttendencePartial.cshtml"));
        }

        public async Task<IActionResult> chekAttendenceDependOnPeriod(int courseId, int batchId)
        {
            List<string> periodsData = new List<string>();
            var courseModel = await _ICourseRepo.GetSingle(x => x.IsActive == 1 && x.IsDeleted == 0 && x.Id == courseId);
            if (courseModel.AttendenceType.Trim().ToLower() != "daily")
            {
                var periods = await _timeSheetRepo.GetAllIncludeTimeSheetDetails(courseId, batchId);
                periods.TimeTableModels.ForEach(item =>
                {
                    item.PeriodModels.ForEach(data =>
                    {
                        periodsData.Add(data.Period);
                    });

                });
                return Json(periodsData.Distinct());
            }
            return Json("0");
        }

        public async Task<IActionResult> StudentAttendence(int courseId, int batchId, DateTime attendenceDate, string period)
        {
            HttpContext.Session.SetString("attendenceDate", attendenceDate.ToShortDateString());
            HttpContext.Session.SetString("period", period?? "0");

            var studentPromoteModels = await _IStudentPromote.GetList(x => x.IsActive == 1 && x.IsDeleted == 0
             && x.CourseId == courseId && x.BatchId == batchId);
            var studentMasterModels = await _IStudentMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            var studentAttendence = await _IStudentAttendence.GetList(x => x.IsActive == 1 && x.IsDeleted == 0
             && x.AttendenceDate == attendenceDate );

            var attendenceType = (await _ICourseRepo.GetSingle(x => x.Id == courseId)).AttendenceType;

            var studentAttVms = (from SP in studentPromoteModels
                                 join SM in studentMasterModels
                                 on SP.StudentId equals SM.Id
                                 select new StudentAttendenceVm
                                 {
                                     StudentId = SP.StudentId,
                                     Name = SM.Name,
                                     RollCode = SM.RollCode,
                                     RegistrationNumber = SM.RegistrationNumber,
                                     StudentImage = SM.StudentPhoto,
                                     AttendenceMarkType = attendenceType
                                 }).ToList();
            studentAttendence.ToList().ForEach(x =>
            {
                studentAttVms.ForEach(item =>
                {
                    if (x.Student == item.StudentId)
                    {
                        item.AttendenceType = x.AttendenceType;
                    }
                });
            });
            HttpContext.Session.SetString("attenType", studentAttVms.First().AttendenceMarkType);
            return PartialView("~/Views/StudentAttendence/_StudentAttendencePartial.cshtml", studentAttVms);

        }

        [HttpPost]
        public async Task<IActionResult> UpSertAttendence()
        {
            var attendType = HttpContext.Session.GetString("attenType");
            var periodId = attendType.Trim().ToLower() == "sw" ? HttpContext.Session.GetString("period") : "All" ;

            var studentIds = (Request.Form["studentId"].ToString().Split(',')).Select(x => Int32.Parse(x)).ToList();
            var attendenceType = Request.Form["AttendType"].ToString().Split(',');
            var date = Convert.ToDateTime(HttpContext.Session.GetString("attendenceDate"));

            var attendenceModels = await _IStudentAttendence.GetList(x => x.AttendenceDate == date
             && x.IsActive == 1 && x.IsDeleted == 0 && studentIds.Contains(x.Student) && x.PeriodId== periodId);

            attendenceModels.ToList().ForEach(x =>
            {
                x.IsActive = 0;
                x.IsDeleted = 1;
            });
            if (attendenceModels.Count() > 0)
            {
                await _IStudentAttendence.CreateNewContext();
                var updateResult = await _IStudentAttendence.Update(attendenceModels.ToArray());

            }
            await _IStudentAttendence.CreateNewContext();
            List<StudentAttendenceModel> models = new List<StudentAttendenceModel>();

            for (int i = 0; i < studentIds.Count(); i++)
            {
                StudentAttendenceModel model = new StudentAttendenceModel();
                model.Student = studentIds[i];
                model.AttendenceDate = date;
                model.AttendenceType = attendenceType[i];
                model.PeriodId = periodId;
                models.Add(model);
            }
            var result = await _IStudentAttendence.Add(models.ToArray());

            return Json(ResponseData.Instance.GenericResponse(result));
        }
    }
}