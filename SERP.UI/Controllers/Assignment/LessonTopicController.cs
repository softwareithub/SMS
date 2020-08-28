using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.HomeAssignment;
using SERP.Core.Entities.LessionMaster;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Assignment
{
    public class LessonTopicController : Controller
    {
        private readonly IGenericRepository<LessonTopicMapping, int> _ILessonTopicRepo;
        private readonly IGenericRepository<LessonMaster, int> _ILessonMasterRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMasterRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        
        public LessonTopicController(IGenericRepository<LessonTopicMapping, int> lessonTopicRepo, IGenericRepository<LessonMaster, int> lessonMasterRepo,
                IGenericRepository<CourseMaster, int> courseMasterRepo, IGenericRepository<SubjectMaster, int> subjectRepo, IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _ILessonTopicRepo = lessonTopicRepo;
            _ICourseMasterRepo = courseMasterRepo;
            _ISubjectRepo = subjectRepo;
            _ILessonMasterRepo = lessonMasterRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> CreateLessonTopic(int id,string subjectName)
        {
            try
            {
                ViewBag.ClassList = await _ICourseMasterRepo.GetList(x => x.IsActive == 1);
                var model = await _ILessonTopicRepo.GetSingle(z => z.Id == id);
                return View("~/Views/AssignmentWork/_lessonTopicPartial.cshtml", model);
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

        public async Task<IActionResult> PostLessonTopic(string [] topic, string[] description, string[] classExpected,int ddlLesson)
        {
            try
            {
                List<LessonTopicMapping> models = new List<LessonTopicMapping>();
                for (int i = 0; i < topic.Count(); i++)
                {
                    LessonTopicMapping model = new LessonTopicMapping();
                    model.LessonId = ddlLesson;
                    model.TopicName = topic[i];
                    model.TopicDescription = description[i];
                    model.ExpectedClass = Convert.ToInt32(classExpected[i]);
                    models.Add(model);
                }
                var response = await _ILessonTopicRepo.Add(models.ToArray());
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

        public async Task<IActionResult> GetLessonDetail(int subjectid)
        {
            try
            {
                var response = await _ILessonMasterRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return Json(response);
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

        public async Task<IActionResult> GetLessonTopics(int lessonId)
        {
            var model = await _ILessonTopicRepo.GetList(x => x.IsActive == 1 && x.LessonId == lessonId);
            return PartialView("~/Views/AssignmentWork/_LessonTopicDetailPartial.cshtml", model);
        }

        public async Task<IActionResult> DeleteLesson(int id)
        {
            var model = await _ILessonTopicRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<LessonTopicMapping>(model, 1);
            await _ILessonTopicRepo.CreateNewContext();
            var response = await _ILessonTopicRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}