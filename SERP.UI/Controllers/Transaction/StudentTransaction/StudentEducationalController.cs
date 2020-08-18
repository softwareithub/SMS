using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Transaction.StudentTransaction
{
    public class StudentEducationalController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IGenericRepository<StudentEducationalDetail, int> _educationalRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public StudentEducationalController(IGenericRepository<StudentMaster, int> studentRepo, IGenericRepository<StudentEducationalDetail, int> educationalRepo, IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
)
        {
            _studentRepo = studentRepo;
            _educationalRepo = educationalRepo;
           _exceptionLoggingRepo = exceptionLoggingRepo;

        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.StudentList = await _studentRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                var educationDetail = await _educationalRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/StudentMaster/_StudentEducationalPartial.cshtml", educationDetail);
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
        public async Task<IActionResult> Create(StudentEducationalDetail model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var response = await _educationalRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(response));
                }
                else
                {
                    var updateModel = CommanDeleteHelper.CommanUpdateCode<StudentEducationalDetail>(model, 1);
                    var response = await _educationalRepo.Update(updateModel);
                    return Json(ResponseData.Instance.GenericResponse(response));
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
        public async Task<IActionResult> EducationDetails()
        {
            try
            {
                var models = (from SM in await _studentRepo.GetList(x => x.IsActive == 1)
                              join ED in await _educationalRepo.GetList(x => x.IsActive == 1)
                              on SM.Id equals ED.StudentId
                              select new StudentEducationalDetail
                              {
                                  Id = ED.Id,
                                  StudentName = SM.Name + " (" + SM.RegistrationNumber + ") ",
                                  University = ED.University,
                                  CollegeName = ED.CollegeName,
                                  CourseName = ED.CourseName,
                                  YearOfJoining = ED.YearOfJoining,
                                  YearOfPassing = ED.YearOfPassing,
                                  EnrollmentNumber = ED.EnrollmentNumber
                              }).ToList();

                return PartialView("~/Views/StudentMaster/_StudentEducationDetailPartial.cshtml", models);
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _educationalRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<StudentEducationalDetail>(model, 1);
                await _educationalRepo.CreateNewContext();
                var response = await _educationalRepo.Update(deleteModel);
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
    }
}