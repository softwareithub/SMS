using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.OnlineTest;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Extension;
using SERP.UI.Models;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.OnlineTest
{
    public class OnlineTestController : Controller
    {
        private readonly IGenericRepository<QuestionModel, int> _QuestionRepo;
        private readonly IGenericRepository<OptionMaster, int> _OptionRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<SMSTemplateModel, int> _smsTemplateRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public OnlineTestController(IGenericRepository<QuestionModel, int> QuestionRepo, IGenericRepository<OptionMaster, int> optionRepo,
             IGenericRepository<CourseMaster, int> courseRepo, ISubjectMasterRepo subjectRepo,
             IGenericRepository<SMSTemplateModel, int> smsTemplateRepo,
             IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
            )
        {
            _QuestionRepo = QuestionRepo;
            _OptionRepo = optionRepo;
            _ICourseRepo = courseRepo;
            _ISubjectRepo = subjectRepo;
            _smsTemplateRepo = smsTemplateRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.CourseList = await _ICourseRepo.GetAll(x => x.IsActive == 1);
                var questionModel = await _QuestionRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/ExamMaster/_QuestionBankPartial.cshtml", questionModel);
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
        public async Task<IActionResult> CreateQuestion(QuestionVm model)
        {
            try
            {
                QuestionOptions optmodel = HttpContext.Session.GetObject<QuestionOptions>("questOptions");
                var response = await _QuestionRepo.CreateEntity(model.QuestionModel);
                await _QuestionRepo.CreateNewContext();
                var questionId = Convert.ToInt32((await _QuestionRepo.GetList(x => x.IsActive == 1)).Max(x => x.Id));
                List<OptionMaster> optionMasters = new List<OptionMaster>();

                optmodel.Options.ForEach(item =>
                {
                    OptionMaster optionMaster = new OptionMaster();
                    optionMaster.QuestionId = questionId;
                    optionMaster.OptionData = item;
                    optionMaster.SortOrder = 0;
                    optionMasters.Add(optionMaster);

                });

                var optionResponse = await _OptionRepo.Add(optionMasters.ToArray());

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

        public async Task<IActionResult> GetList()
        {
            try
            {
                var questionModels = await _QuestionRepo.GetList(x => x.IsActive == 1);

                return PartialView("~/Views/ExamMaster/_QuestionDetailPartial.cshtml", questionModels);
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

        public async Task<IActionResult> GetSMSEmailTemplate(string notificationType)
        {
            try
            {
                var model = await _smsTemplateRepo.GetList(x => x.TemplateType == notificationType);
                return Json(model);
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

        public async Task<IActionResult> AddOptions(int id)
        {
            try
            {
                ViewBag.QuestionList = await _QuestionRepo.GetList(x => x.IsActive == 1);
                var model = await _OptionRepo.GetSingle(x => x.Id == id);
                return await Task.Run(() => View("~/Views/ExamMaster/_AddOptionToQuestion.cshtml", model));
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
        public async Task<IActionResult> CreateOptions(OptionMaster optionMaster)
        {
            try
            {
                var response = await _OptionRepo.CreateEntity(optionMaster);
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


        public async Task<IActionResult> GetQuestionOptionList(int questId)
        {
            try
            {
                var models = await _OptionRepo.GetList(x => x.QuestionId == questId && x.IsActive == 1);
                return PartialView("~/Views/ExamMaster/_QuestionOptionPartial.cshtml", models.OrderBy(x => x.SortOrder));
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

        public async Task<IActionResult> UpdateOptionCorrect(int id, int correctOption)
        {
            try
            {
                var model = await _OptionRepo.GetSingle(x => x.Id == id);
                model.IsCorrectAnswere = correctOption;
                await _OptionRepo.CreateNewContext();
                var updateResponse = await _OptionRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(updateResponse));
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

        public async Task<IActionResult> UpdateSortOrder(int id, int sortOrder)
        {
            try
            {
                var model = await _OptionRepo.GetSingle(x => x.Id == id);
                model.SortOrder = sortOrder;
                await _OptionRepo.CreateNewContext();
                var updateResponse = await _OptionRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(updateResponse));
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

        public async Task<IActionResult> DeleteOption(int id)
        {
            try
            {
                var model = await _OptionRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<OptionMaster>(model, 1);
                await _OptionRepo.CreateNewContext();
                var response = await _OptionRepo.Update(deleteModel);
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

        public async Task<IActionResult> AddOptionTemp(string question, string option)
        {

            if (HttpContext.Session.GetObject<QuestionOptions>("questOptions") == null)
            {
                List<string> options = new List<string>();
                QuestionOptions model = new QuestionOptions();
                model.Question = question;
                options.Add(option);
                model.Options = options;

                HttpContext.Session.SetObject("questOptions", model);

                return PartialView("~/Views/OnlineTest/QuestionOptionsTempPartial.cshtml", model);
            }
            else
            {
                QuestionOptions model = HttpContext.Session.GetObject<QuestionOptions>("questOptions");
                model.Options.Add(option);
                HttpContext.Session.Remove("questOptions");
                HttpContext.Session.SetObject("questOptions", model);

                return PartialView("~/Views/OnlineTest/QuestionOptionsTempPartial.cshtml", model);
            }
        }
    }
}