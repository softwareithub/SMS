using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LessionMaster;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.AssignmentHomeModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Assignment
{
    public class LessonMasterController : Controller
    {
        private readonly IGenericRepository<LessonMaster, int> _ILessonMasterRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMasterRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        private readonly ILessonRepository _ILessonRepository;
        public LessonMasterController(IGenericRepository<LessonMaster, int> lessonMasterRepo,
                IGenericRepository<CourseMaster, int> courseMasterRepo, IGenericRepository<SubjectMaster, int> subjectRepo,
                IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo, ILessonRepository lessonRepository)
        {
            _ILessonMasterRepo = lessonMasterRepo;
            _ICourseMasterRepo = courseMasterRepo;
            _ISubjectRepo = subjectRepo;
            _ILessonRepository = lessonRepository;
        }
        public async Task<IActionResult> CreateLesson(int id)
        {
            try
            {
                var model = await _ILessonMasterRepo.GetSingle(x => x.Id == id);
                ViewBag.ClassList = await _ICourseMasterRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/AssignmentWork/_LessonMasterPartial.cshtml", model);
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
        public async Task<IActionResult> CreateLesson(LessonMaster model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now;
                    var response = await _ILessonMasterRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(response));
                }
                else
                {
                    var response = await _ILessonMasterRepo.CreateEntity(model);
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

        public async Task<IActionResult> GetLessonPlanning()
        {
            try
            {
                var result = (from LM in await _ILessonMasterRepo.GetList(x => x.IsActive == 1)
                              join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                              on LM.SubjectId equals SM.Id
                              select new LessonPlanningModel
                              {
                                  LessonId = LM.Id,
                                  LessonMasterModel = new LessonMaster()
                                  {
                                      LessonName = LM.LessonName,
                                      StartDate = LM.StartDate,
                                      EndDate = LM.EndDate,
                                      ClassTestDate = LM.ClassTestDate
                                  },
                                  SubjectName = SM.SubjectName

                              }).ToList();

                return PartialView("~/Views/AssignmentWork/_LessionDetailPartial.cshtml", result);
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

        public async Task<IActionResult> GetSubjectList(int courseId)
        {
            try
            {
                return Json(await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.CourseId == courseId));
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _ILessonMasterRepo.GetSingle(x => x.Id == id);
                model.IsActive = 0;
                model.IsDeleted = 1;
                await _ILessonMasterRepo.CreateNewContext();
                var response = await _ILessonMasterRepo.Update(model);
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

        public async Task<IActionResult> GetLessonDetails()
        {
            var model = await _ILessonRepository.GetLessonMasterAsync();
            return PartialView("~/Views/AssignmentWork/_LessonDetailPartial.cshtml", model);
        }
    }
}