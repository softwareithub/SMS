using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Models;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.OnlineTest
{
    public class OnlineTestController : Controller
    {
        private readonly IGenericRepository<QuestionModel, int> _QuestionRepo;
        private readonly IGenericRepository<OptionMaster, int> _OptionRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly ISubjectMasterRepo _ISubjectRepo;

        public OnlineTestController(IGenericRepository<QuestionModel, int> QuestionRepo, IGenericRepository<OptionMaster, int> optionRepo,
             IGenericRepository<CourseMaster, int> courseRepo, ISubjectMasterRepo subjectRepo
            )
        {
            _QuestionRepo = QuestionRepo;
            _OptionRepo = optionRepo;
            _ICourseRepo = courseRepo;
            _ISubjectRepo = subjectRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.CourseList = await _ICourseRepo.GetAll(x => x.IsActive == 1);
            QuestionVm model = new QuestionVm();
            var questionModel = await _QuestionRepo.GetSingle(x => x.Id == id);
            model.QuestionModel = null;
            if (questionModel != null)
            {
                var optionList = await _OptionRepo.GetList(x => x.QuestionId == questionModel.Id && x.IsActive == 1);
                model.QuestionModel = questionModel;
                model.OptionMasters = optionList.ToList();
            }

            return PartialView("~/Views/ExamMaster/_QuestionBankPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(QuestionVm model)
        {
            var isCorrect = Request.Form["IsCorrectAnswere"];
            if (model.QuestionModel.Id == 0)
            {
                var count = 1;
                var requestOptions = Request.Form["OptionData"];

                List<OptionMaster> optionModels = new List<OptionMaster>();

                var response = await _QuestionRepo.CreateEntity(model.QuestionModel);
                await _QuestionRepo.CreateNewContext();
                var id = (await _QuestionRepo.GetList(x => x.IsActive == 1)).Max(x => x.Id);

                foreach (var data in requestOptions.ToString().Split(','))
                {
                    OptionMaster optionModel = new OptionMaster();
                    optionModel.OptionData = data;
                    optionModel.QuestionId = id;
                    if (count == Convert.ToInt32(isCorrect))
                    {
                        optionModel.IsCorrectAnswere = 1;
                    }
                    else
                    {
                        optionModel.IsCorrectAnswere = 0;
                    }

                    optionModels.Add(optionModel);
                    count++;
                }

                var optionResponse = await _OptionRepo.Add(optionModels.ToArray());
                return Json(ResponseData.Instance.GenericResponse(optionResponse));
            }
            else
            {
                var response = await UpdateQuestionDetails(model, isCorrect);
                return Json("Data updated successfully");
            }

        }

        public async Task<IActionResult> GetList()
        {
            var questionModels = await _QuestionRepo.GetList(x => x.IsActive == 1);
            var optionModesl = await _OptionRepo.GetList(x => x.IsActive == 1);
            var data = (from QM in questionModels
                        join OM in optionModesl
                        on QM.Id equals OM.QuestionId
                        select new
                        {
                            questionId = QM.Id,
                            question = QM.Question,
                            QuestionPoint = QM.QuestionPoint,
                            optionData = OM.OptionData,
                            IsCorrectAnswere = OM.IsCorrectAnswere,
                        }).ToList();
            List<QuestionVm> questonVms = new List<QuestionVm>();
            foreach (var opt in data.GroupBy(x => x.question))
            {
                QuestionVm questionVm = new QuestionVm();
                QuestionModel quest = new QuestionModel();
                quest.Id = opt.First().questionId;
                quest.Question = opt.First().question;
                quest.QuestionPoint = opt.First().QuestionPoint;
                QuestionVm questVm = new QuestionVm();
                questVm.QuestionModel = quest;
                questionVm.QuestionModel = quest;

                List<OptionMaster> optModels = new List<OptionMaster>();

                foreach (var optData in opt)
                {
                    OptionMaster optModel = new OptionMaster();
                    optModel.OptionData = optData.optionData;
                    optModel.IsCorrectAnswere = optData.IsCorrectAnswere;
                    optModel.QuestionId = opt.First().questionId;
                    optModels.Add(optModel);
                }
                questVm.OptionMasters = optModels;
                questonVms.Add(questVm);
            }

            return PartialView("~/Views/ExamMaster/_QuestionDetailPartial.cshtml", questonVms);
        }

        private async Task<bool> UpdateQuestionDetails(QuestionVm model, string correctAnswere)
        {
            var updateQuestion = await _QuestionRepo.Update(model.QuestionModel);
            int count = 1;
            model.OptionMasters.ForEach(item => {
                item.QuestionId = model.QuestionModel.Id;
                if (count == Convert.ToInt32(correctAnswere))
                {
                    item.IsCorrectAnswere = 1;
                }
                else
                {
                    item.IsCorrectAnswere = 0;
                }
                count++;
            });
            var response = await _OptionRepo.Update(model.OptionMasters.ToArray());
            return true;
        }
    }
}