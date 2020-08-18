using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Models;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Transaction.StudentTransaction
{
    public class StudentPromoteController : Controller
    {
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromoteRepo;
        public readonly IGenericRepository<StudentMaster, int> _IStudentMasterRepo;
        public readonly IGenericRepository<CourseMaster, int> _ICourseMasterRepo;
        public readonly IGenericRepository<BatchMaster, int> _IBatchMasterRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;


        public StudentPromoteController(IGenericRepository<StudentPromote, int> studentRepo,
            IGenericRepository<StudentMaster, int> studentMasterRepo, IGenericRepository<CourseMaster, int> courseRepo,
            IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
            )
        {
            _IStudentMasterRepo = studentMasterRepo;
            _IStudentPromoteRepo = studentRepo;
            _ICourseMasterRepo = courseRepo;
            _IBatchMasterRepo = batchRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> PromoteStudent()
        {
            try
            {
                ViewBag.CourseList = await _ICourseMasterRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                ViewBag.BatchList = await _IBatchMasterRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
                return PartialView("~/Views/StudentPromote/_StudentPromotePartial.cshtml");
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

        public async Task<IActionResult> GetStudentList(int courseId, int batchId)
        {
            try
            {
                var promotedStudentModels = await _IStudentPromoteRepo.GetList(x => x.CourseId == courseId &&
                 x.BatchId == batchId && x.IsActive == 1 && x.IsDeleted == 0);
                await _IStudentMasterRepo.CreateNewContext();
                var studentMasterModels = await _IStudentMasterRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);

                var models = (from stdPro in promotedStudentModels
                              join stdModel in studentMasterModels
                              on stdPro.StudentId equals stdModel.Id
                              where stdModel.IsActive == 1 && stdModel.IsDeleted == 0
                              && stdPro.IsDeleted == 0 && stdPro.IsActive == 1
                              select new StudentPromoteVm
                              {
                                  StudentId = stdModel.Id,
                                  StudentName = stdModel.Name,
                                  RollCode = stdModel.RollCode,
                                  Registration = stdModel.RegistrationNumber,
                                  Phone = stdModel.StudentPhone,
                                  PromoteDate = stdPro.PromotionDate
                              }).ToList();

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

        public async Task<IActionResult> PostStudentPromote(int []studentIds, int courseId, int batchId, int prevBatchId, int prevCourseId)
        {
            try
            {
                var intialPrmoteModels = await _IStudentPromoteRepo.GetList(x => x.CourseId == prevCourseId && x.BatchId == prevBatchId &&
                    studentIds.Contains(x.StudentId));
                intialPrmoteModels.ToList().ForEach(x =>
                {
                    x.IsActive = 0;
                    x.IsDeleted = 1;
                    x.UpdatedBy = 1;
                    x.UpdatedDate = DateTime.Now.Date;
                });

                await _IStudentPromoteRepo.CreateNewContext();

                var updateResult = await _IStudentPromoteRepo.Update(intialPrmoteModels.ToArray<StudentPromote>());

                List<StudentPromote> models = new List<StudentPromote>();

                for (int i = 0; i < studentIds.Count(); i++)
                {
                    StudentPromote model = new StudentPromote
                    {
                        StudentId = studentIds[i],
                        CourseId = courseId,
                        BatchId = batchId,
                        IsActive = 1,
                        IsDeleted = 0,
                        PromotionDate = DateTime.Now.Date
                    };
                    models.Add(model);
                }

                await _IStudentPromoteRepo.CreateNewContext();

                var result = await _IStudentPromoteRepo.Add(models.ToArray());

                return Json(ResponseData.Instance.GenericResponse(result));
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