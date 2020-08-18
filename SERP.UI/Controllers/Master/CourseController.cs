using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Master
{
    public class CourseController : Controller
    {
        private readonly IGenericRepository<CourseMaster, int> _IGenericRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public CourseController(IGenericRepository<CourseMaster, int> genericRepository,
                                IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IGenericRepo = genericRepository;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id = 0)
        {
            try
            {
                if (id == 0)
                    return await Task.Run(() => PartialView("~/Views/CourseMaster/_CourseMasterPartial.cshtml"));

                var courseDetail = await _IGenericRepo.GetSingle(x => x.Id == id);
                var attendenceType = courseDetail.AttendenceType.Trim();
                courseDetail.AttendenceType = attendenceType;
                return PartialView("~/Views/CourseMaster/_CourseMasterPartial.cshtml", courseDetail);
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
        public async Task<IActionResult> Create(CourseMaster model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var createModel = CommanDeleteHelper.CommanCreateCode<CourseMaster>(model,1);
                    var result = await _IGenericRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now.Date;
                    var result = await _IGenericRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
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

        [HttpGet]
        public async Task<IActionResult> GetCourseList()
        {
            try
            {
                var result = await _IGenericRepo.GetList(c => c.IsActive == 1 && c.IsDeleted == 0);
                return PartialView(PartialPages.PartialPageDetails.ClassDetailPartial, result);
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

        [HttpGet]
        public async Task<IActionResult> DeleteCourse(int Id)
        {
            try
            {
                var course = await _IGenericRepo.GetSingle(x => x.Id == Id);
                course.IsActive = 0;
                course.IsDeleted = 1;
                course.UpdatedBy = 1;
                course.UpdatedDate = DateTime.Now.Date;
                await _IGenericRepo.CreateNewContext();
                var result = await _IGenericRepo.Delete(course);
                return Json("Record deleted successfully.");
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