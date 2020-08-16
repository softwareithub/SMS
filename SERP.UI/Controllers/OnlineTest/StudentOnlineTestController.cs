using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.OnlineTest;
using SERP.Core.Model.OnlineTest;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Extension;

namespace SERP.UI.Controllers.OnlineTest
{
    public class StudentOnlineTestController : Controller
    {
        public static List<TestSequenceModel> testSequenceModels;

        private List<string> questions = new List<string>();
        private readonly IGenericRepository<TestMaster, int> _testMasterRepo;
        private readonly IGenericRepository<QuestionModel, int> _questionRepo;
        private readonly IGenericRepository<OptionMaster, int> _optionRepo;
        private readonly IGenericRepository<TestQuestionMapping, int> _testQuestionMappingRepo;
        private readonly IGenericRepository<UserTestDetailModel, int> _userDetailRepo;
        private readonly IGenericRepository<UserAnswereSheetModel, int> _userAnswereSheetModelRepo;
        private readonly IOnlineTestSubmitRepository _onlineTestSubmitRepository;

        public StudentOnlineTestController(IGenericRepository<TestMaster, int> testMasterRepo, IGenericRepository<TestQuestionMapping, int> testQuestionMappingRepo, IGenericRepository<QuestionModel, int> questionRepo,
        IGenericRepository<OptionMaster, int> optionRepo, IGenericRepository<UserTestDetailModel, int> userTestDetailRepo, IGenericRepository<UserAnswereSheetModel, int> userAnswereSheetRepo, IOnlineTestSubmitRepository onlineTestSubmitRepository)
        {
            _testMasterRepo = testMasterRepo;
            _testQuestionMappingRepo = testQuestionMappingRepo;
            _questionRepo = questionRepo;
            _optionRepo = optionRepo;
            _userDetailRepo = userTestDetailRepo;
            _userAnswereSheetModelRepo = userAnswereSheetRepo;
            _onlineTestSubmitRepository = onlineTestSubmitRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _testMasterRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/StudentOnlineTest/OnlineTestStudentPartial.cshtml", model);
        }

        public async Task<IActionResult> GetTestRuleAndRegulation(int testId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var model = await _testMasterRepo.GetSingle(x => x.Id == testId);
            HttpContext.Session.SetString("TestLimit", model.TestTimeLimit);
            var userTestDetail = await _userDetailRepo.GetSingle(x => x.TestId == testId && x.UserId == userId);

            return PartialView("~/Views/StudentOnlineTest/OnlineExamRuleRegulationPartial.cshtml", model);

            if (userTestDetail == null && string.IsNullOrEmpty(userTestDetail?.TestStatus))
            {
                return PartialView("~/Views/StudentOnlineTest/OnlineExamRuleRegulationPartial.cshtml", model);
            }
            if (userTestDetail.TestStatus.Trim().ToLower() == "started")
            {
                return PartialView("~/Views/StudentOnlineTest/OnlineExamRuleRegulationPartial.cshtml", model);
            }
            else
            {
                TestMaster testMaster = new TestMaster();
                testMaster.Regulation = "Test has been Expired.";
                return PartialView("~/Views/StudentOnlineTest/OnlineExamRuleRegulationPartial.cshtml", testMaster);
            }

        }

        public async Task<IActionResult> TestQuestions(int testId)
        {

            var onlineExamdetails = await GetQuestionByTest(testId);
            var userId = HttpContext.Session.GetInt32("UserId");
            HttpContext.Session.SetInt32("testId", testId);

            var userTestModel = new UserTestDetailModel()
            {
                TestId = testId,
                UserId = Convert.ToInt32(userId),
                DateOfExamination = DateTime.Now,
                TestStatus = "Started"

            };
            var response = await _userDetailRepo.CreateEntity(userTestModel);
            var userDetailId = (await _userDetailRepo.GetList(x => x.IsActive == 1)).Max(x => x.Id);
            HttpContext.Session.SetInt32("userTestId", userDetailId);
            return PartialView("~/Views/StudentOnlineTest/_OnlineExamStartPartial.cshtml", onlineExamdetails);

        }
        [HttpPost]
        public async Task<IActionResult> SaveQuestion(int Question, string[] Option, int Sequence)
        {

            var onlineExamdetails = await GetQuestionByTest(Convert.ToInt32(HttpContext.Session.GetInt32("testId")));
            int lastSequence = onlineExamdetails.Max(x => x.Sequence);

            if (Option.Count() > 0)
            {
                var isUpdated = await CHeckQuestionAnsExists(Question);

                if (HttpContext.Session.GetObject<List<string>>("questionIds") == null)
                {
                    questions.Add(Question.ToString());
                    HttpContext.Session.SetObject("questionIds", questions);
                }
                else
                {
                    questions = HttpContext.Session.GetObject<List<string>>("questionIds");
                    questions.Add(Question.ToString());
                    HttpContext.Session.SetObject("questionIds", questions);
                }
            }
            else
            {
                questions = HttpContext.Session.GetObject<List<string>>("questionIds");
                questions.Remove(Question.ToString());
                HttpContext.Session.SetObject("questionIds", questions);
            }


            List<UserAnswereSheetModel> models = new List<UserAnswereSheetModel>();
            for (int i = 0; i < Option.Count(); i++)
            {
                var userAnswereSheet = new UserAnswereSheetModel()
                {
                    ChooseOptionId = Convert.ToInt32(Option[i]),
                    UserTestDetailId = Convert.ToInt32(HttpContext.Session.GetInt32("userTestId")),
                    QuestionId = Question,
                    IsAttempted = true
                };
                models.Add(userAnswereSheet);
            }

            await _userAnswereSheetModelRepo.CreateNewContext();
            var insertAnswereSheet = await _userAnswereSheetModelRepo.Add(models.ToArray());


            if (Sequence == lastSequence)
            {
                if (questions.Count >= lastSequence)
                {
                    int testId = Convert.ToInt32(HttpContext.Session.GetInt32("testId"));
                    var userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    var model = await GetFinalSubmisionModel(userId, testId);
                    return PartialView("~/Views/OnlineTest/_TestSubmissionPartialView.cshtml", model);
                }
                else
                {
                    ModelState.Remove("QuestionId");
                    ModelState.Remove("Id");
                    ModelState.Remove("Sequence");
                    ModelState.Remove("Question");
                    Sequence = 1;
                    var model = onlineExamdetails.Where(x => x.Sequence == Sequence).FirstOrDefault();
                    int testId = Convert.ToInt32(HttpContext.Session.GetInt32("testId"));

                    return await Task.Run(() => PartialView("~/Views/StudentOnlineTest/_ExamQuestionPartial.cshtml", model));
                }

            }
            else
            {
                ModelState.Remove("QuestionId");
                ModelState.Remove("Id");
                ModelState.Remove("Sequence");
                ModelState.Remove("Question");
                Sequence++;
                var model = onlineExamdetails.Where(x => x.Sequence == Sequence).FirstOrDefault();
                int testId = Convert.ToInt32(HttpContext.Session.GetInt32("testId"));

                return await Task.Run(() => PartialView("~/Views/StudentOnlineTest/_ExamQuestionPartial.cshtml", model));

            }
        }

        private async Task<bool> CHeckQuestionAnsExists(int questionId)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int testId = Convert.ToInt32(HttpContext.Session.GetInt32("testId"));
            var model = (from UD in await _userDetailRepo.GetList(x => x.TestId == testId && x.UserId == userId)
                         join SAS in await _userAnswereSheetModelRepo.GetList(x => x.IsActive == 1)
                         on UD.Id equals SAS.UserTestDetailId
                         where SAS.QuestionId == questionId
                         select new
                         {
                             SAS.Id
                         }.Id).ToList();

            await _userAnswereSheetModelRepo.CreateNewContext();

            var sasModel = await _userAnswereSheetModelRepo.GetList(x => model.Contains(x.Id));

            if (sasModel != null)
            {
                await _userAnswereSheetModelRepo.CreateNewContext();
                sasModel.ToList().ForEach(item =>
                {
                    item.IsActive = 0;
                    item.IsDeleted = 1;
                });
                var updateResponse = await _userAnswereSheetModelRepo.Update(sasModel.ToArray());
                return false;
            }
            return true;

        }

        public async Task<IActionResult> GetQuestion(int questionId)
        {
            int testId = Convert.ToInt32(HttpContext.Session.GetInt32("testId"));
            var onlineExamdetails = await GetQuestionByTest(Convert.ToInt32(HttpContext.Session.GetInt32("testId")));

            await CheckUserSelectAnswere(testId, onlineExamdetails);
            var model = onlineExamdetails.Where(x => x.QuestionId == questionId).FirstOrDefault();
            return await Task.Run(() => PartialView("~/Views/StudentOnlineTest/_ExamQuestionPartial.cshtml", model));
        }
        private async Task<List<OnLineExamDetail>> GetQuestionByTest(int testId)
        {

            var onlineExamdetails = (from TQM in await _testQuestionMappingRepo.GetList(x => x.TestId == testId && x.IsActive == 1)
                                     join QM in await _questionRepo.GetList(x => x.IsActive == 1)
                                     on TQM.QuestionId equals QM.Id
                                     orderby (TQM.Id)
                                     select new OnLineExamDetail
                                     {
                                         Id = TQM.Id,
                                         QuestionId = QM.Id,
                                         Question = QM.Question,
                                         QuestionPoint = QM.QuestionPoint,
                                         AnswereType=QM.AnswereType

                                     }).ToList();

            var optionsModels = await _optionRepo.GetList(x => x.IsActive == 1);

            int x = 0;
            onlineExamdetails.ForEach(item =>
            {
                x++;
                List<Options> options = new List<Options>();

                optionsModels.ToList().ForEach(op =>
                {
                    if (item.QuestionId == op.QuestionId)
                    {
                        Options optionModel = new Options()
                        {
                            OptionId = op.Id,
                            Option = op.OptionData,
                            IsSelected = false

                        };
                        options.Add(optionModel);
                    }

                });
                item.Options = options;
                item.Sequence = x;
            });



            return onlineExamdetails;
        }

        private async Task CheckUserSelectAnswere(int testId, List<OnLineExamDetail> onlineExamdetails)
        {
            //Check User has Insert the Answere for the question or Not 

            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            var questAnsSheet = (from UD in await _userDetailRepo.GetList(x => x.IsActive == 1 && x.UserId == userId && x.TestId == testId)
                                 join SAS in await _userAnswereSheetModelRepo.GetList(x => x.IsActive == 1)
                                 on UD.Id equals SAS.UserTestDetailId
                                 select new
                                 {
                                     QuestionId = SAS.QuestionId,
                                     SelectOptionId = SAS.ChooseOptionId

                                 }).ToList();

            if (questAnsSheet.Count() > 0)
            {
                onlineExamdetails.ForEach(item =>
                {
                    questAnsSheet.ForEach(q =>
                    {
                        if (item.QuestionId == q.QuestionId)
                        {
                            item.Options.ForEach(op =>
                            {
                                if (op.OptionId == q.SelectOptionId)
                                {
                                    op.IsSelected = true;
                                }
                            });
                        }
                    });

                });
            }
        }

        private async Task<TestSubmissionModel> GetFinalSubmisionModel(int userId, int testId)
        {
            return await _onlineTestSubmitRepository.GetOnlineSubmitDetail(userId, testId);
        }

        public async Task<IActionResult> SubmitTest()
        {
            int testId = Convert.ToInt32(HttpContext.Session.GetInt32("testId"));
            var userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var model = await GetFinalSubmisionModel(userId, testId);
            model.Message = "The Test  " + model.TestName + " has been submitted";

            var testModel = await _userDetailRepo.GetSingle(x => x.TestId == testId && x.UserId == userId);
            testModel.TestStatus = "User End Test";
            await _userDetailRepo.CreateNewContext();
            var response = await _userDetailRepo.Update(testModel);

            return PartialView("~/Views/OnlineTest/_TestSubmissionPartialView.cshtml", model);
        }


    }
}