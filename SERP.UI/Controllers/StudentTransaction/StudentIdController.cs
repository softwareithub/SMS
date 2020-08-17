using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.MasterViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;

namespace SERP.UI.Controllers.StudentTransaction
{
    public class StudentIdController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _IStudentMasterRepo;
        private readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        

        public StudentIdController(IGenericRepository<StudentMaster, int> studentRepo, IGenericRepository<StudentPromote, int>studentPromote, IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<BatchMaster, int> batchRepo
            , IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IStudentMasterRepo = studentRepo;
            _IStudentPromote = studentPromote;
            _ICourseRepo = courseRepo;
            _IBatchRepo = batchRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/StudentIDCard/_StudentIDCardPartial.cshtml");
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

        public async Task<IActionResult>GetStudentIDCard(int  batchId)
        {
            try
            {
                List<StudentPartialInfoViewModel> models = new List<StudentPartialInfoViewModel>();
                models = (from SP in await _IStudentPromote.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.BatchId == batchId)
                          join SM in await _IStudentMasterRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1)
                          on SP.StudentId equals SM.Id
                          join CM in await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0)
                          on SP.CourseId equals CM.Id
                          join BM in await _IBatchRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0)
                          on SP.BatchId equals BM.Id
                          select new StudentPartialInfoViewModel
                          {
                              Id = SM.Id,
                              StudentName = SM.Name,
                              FatherName = SM.FatherName,
                              RollCode = SM.RollCode,
                              CourseName = CM.Name,
                              BatchName = BM.BatchName,
                              FatherPhone = SM.FatherPhone,
                              StudentPhoto = SM.StudentPhoto,
                              C_Address = SM.C_Address
                          }).ToList();

                return PartialView("~/Views/StudentIDCard/_StudentIDCardDetails.cshtml", models);
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
    }
}