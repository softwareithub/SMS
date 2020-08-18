using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.CommanJson
{
    public class CommanDataForJsonController : Controller
    {
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly ITimeSheetRepo _timeSheetRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _basicInfoRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public CommanDataForJsonController(ISubjectMasterRepo subjectRepo,
                                        IGenericRepository<BatchMaster, int> batchRepo,
                                        ITimeSheetRepo timeSheetRepo, 
                                        IGenericRepository<EmployeeBasicInfoModel, int> basicInfoRepo,
                                        IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
                                        )
        {
            _ISubjectRepo = subjectRepo;
            _IBatchRepo = batchRepo;
            _timeSheetRepo = timeSheetRepo;
            _basicInfoRepo = basicInfoRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> SubjectJson(int courseId)
        {
            try
            {
                var result = await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.CourseId == courseId);
                return Json(result);
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
        public async Task<IActionResult> BatchJson(int courseId)
        {
            try
            {
                var result = await _IBatchRepo.GetList(x => x.IsActive == 1 && x.CourseId == courseId);
                return Json(result);
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
        public async Task<IActionResult> TeacherJson(int subjectId)
        {
            try
            {
                var result = await _timeSheetRepo.GetSubjectTeacher(subjectId);
                return Json(result);
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

        public async Task<IActionResult> TeacherJson()
        {
            try
            {
                var result = await _basicInfoRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return Json(result);
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