using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Models;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.OnlineTest
{
    public class OnlineTestController : Controller
    {
        private readonly IGenericRepository<QuestionModel, int> _QuestionRepo;
        private readonly IGenericRepository<OptionMaster, int> _OptionRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<SMSTemplateModel, int> _smsTemplateRepo;

        public OnlineTestController(IGenericRepository<QuestionModel, int> QuestionRepo, IGenericRepository<OptionMaster, int> optionRepo,
             IGenericRepository<CourseMaster, int> courseRepo, ISubjectMasterRepo subjectRepo,
             IGenericRepository<SMSTemplateModel, int> smsTemplateRepo
            )
        {
            _QuestionRepo = QuestionRepo;
            _OptionRepo = optionRepo;
            _ICourseRepo = courseRepo;
            _ISubjectRepo = subjectRepo;
            _smsTemplateRepo = smsTemplateRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.CourseList = await _ICourseRepo.GetAll(x => x.IsActive == 1);
            var questionModel = await _QuestionRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/ExamMaster/_QuestionBankPartial.cshtml", questionModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(QuestionModel model)
        {
            if (model.Id == 0)
            {
                var response = await _QuestionRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<QuestionModel>(model, 1);
                var response = await _QuestionRepo.Update(updateModel);
                return Json(ResponseData.Instance.GenericResponse(response));
            }

        }

        public async Task<IActionResult> GetList()
        {
            var questionModels = await _QuestionRepo.GetList(x => x.IsActive == 1);

            return PartialView("~/Views/ExamMaster/_QuestionDetailPartial.cshtml", questionModels);
        }

        public async Task<IActionResult> GetSMSEmailTemplate(string notificationType)
        {
            var model = await _smsTemplateRepo.GetList(x => x.TemplateType == notificationType);
            return Json(model);
        }

        public async Task<IActionResult> AddOptions(int id)
        {
            ViewBag.QuestionList = await _QuestionRepo.GetList(x => x.IsActive == 1);
            var model = await _OptionRepo.GetSingle(x => x.Id == id);
            return await Task.Run(() => View("~/Views/ExamMaster/_AddOptionToQuestion.cshtml",model));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOptions(OptionMaster optionMaster)
        {
            var response = await _OptionRepo.CreateEntity(optionMaster);
            return Json(ResponseData.Instance.GenericResponse(response));
        }

        public async Task<IActionResult> GetQuestionOptionList(int questId)
        {
            var models = await _OptionRepo.GetList(x => x.QuestionId == questId && x.IsActive == 1);
            return PartialView("~/Views/ExamMaster/_QuestionOptionPartial.cshtml",models);
        }

        public async Task<IActionResult> UpdateOptionCorrect(int id,int correctOption)
        {
            var model = await _OptionRepo.GetSingle(x => x.Id == id);
            model.IsCorrectAnswere = correctOption;
            await _OptionRepo.CreateNewContext();
            var updateResponse = await _OptionRepo.Update(model);
            return Json(ResponseData.Instance.GenericResponse(updateResponse));
        }

        public async Task<IActionResult> DeleteOption(int id)
        {
            var model = await _OptionRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<OptionMaster>(model, 1);
            await _OptionRepo.CreateNewContext();
            var response = await _OptionRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}