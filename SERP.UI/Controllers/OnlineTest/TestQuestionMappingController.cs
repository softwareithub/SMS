using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.OnlineTest;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.ExamModel;
using SERP.Core.Model.OnlineTest;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.EmailHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SERP.UI.Controllers.OnlineTest
{
    public class TestQuestionMappingController : Controller
    {
        private readonly IGenericRepository<QuestionModel, int> _IQuestionRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectMasterRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<TestMaster, int> _ITestRepo;
        private readonly IGenericRepository<TestQuestionMapping, int> _ITestQuestionRepo;
        private readonly IGenericRepository<SMSTemplateModel, int> _smsTemplateRepo;
        private readonly IGenericRepository<StudentPromote, int> _studentPromoteRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentMasterRepo;
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        private readonly IGenericRepository<OptionMaster, int> _optionMasterrepo;

        public TestQuestionMappingController(IGenericRepository<QuestionModel, int> questionRepo, IGenericRepository<SubjectMaster, int> subjectReo, IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<TestMaster, int> testRepo, IGenericRepository<TestQuestionMapping, int> testQuestionRepo, IGenericRepository<SMSTemplateModel, int> smsTemplateRepo, IGenericRepository<StudentPromote, int> studentPromoteRepo, IGenericRepository<StudentMaster, int> studentMasterRepo, IHostingEnvironment hostingEnviroment, IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo, IGenericRepository<OptionMaster, int> optionsMasterRepo)
        {
            _IQuestionRepo = questionRepo;
            _ISubjectMasterRepo = subjectReo;
            _ICourseRepo = courseRepo;
            _ITestRepo = testRepo;
            _ITestQuestionRepo = testQuestionRepo;
            _smsTemplateRepo = smsTemplateRepo;
            _studentPromoteRepo = studentPromoteRepo;
            _studentMasterRepo = studentMasterRepo;
            _hostingEnviroment = hostingEnviroment;
            _exceptionLoggingRepo = exceptionLoggingRepo;
            _optionMasterrepo = optionsMasterRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var courseDetails = await _ICourseRepo.GetList(x => x.IsActive == 1);
                var subjectDetails = await _ISubjectMasterRepo.GetList(x => x.IsActive == 1);
                var testDetails = await _ITestRepo.GetList(x => x.IsActive == 1);
                var questionDetails = await _IQuestionRepo.GetList(x => x.IsActive == 1);
                var options = await _optionMasterrepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

                var response = (from QM in questionDetails
                                join CM in courseDetails
                                on QM.CourseId equals CM.Id
                                join SM in subjectDetails
                                on QM.SubjectId equals SM.Id
                                select new TestQuestionModel
                                {
                                    QuestionId = QM.Id,
                                    TestId = 0,
                                    CourseName = CM.Name,
                                    SubjectName = SM.SubjectName,
                                    Question = QM.Question,
                                    QuestionPoint = QM.QuestionPoint,
                                }).ToList();

                ViewBag.CourseDetail = courseDetails;

                ViewBag.TestDetail = testDetails;

                return PartialView("~/Views/OnlineTest/_TestQuestionMappingPartial.cshtml", response);
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

        public async Task<IActionResult> GetSubjectDetail(int courseId)
        {
            var responseData = await _ISubjectMasterRepo.GetList(x => x.CourseId == courseId && x.IsActive == 1);
            return Json(responseData);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(int TestId, int[] QuestionId, int questionMark)
        {
            try
            {
                if (TestId != 0)
                {
                    var testQuestionList = await _ITestQuestionRepo.GetList(x => x.TestId == TestId);

                    testQuestionList.ToList().ForEach(item =>
                    {
                        item.IsActive = 0;
                        item.IsDeleted = 1;
                    });

                    await _ITestQuestionRepo.CreateNewContext();

                    var updateTestRepo = await _ITestQuestionRepo.Update(testQuestionList.ToArray());

                    List<TestQuestionMapping> models = new List<TestQuestionMapping>();
                    for (int i = 0; i < QuestionId.Count(); i++)
                    {
                        TestQuestionMapping model = new TestQuestionMapping();
                        model.QuestionId = QuestionId[i];
                        model.TestId = TestId;
                        models.Add(model);
                    }

                    await _ITestQuestionRepo.CreateNewContext();

                    var response = await _ITestQuestionRepo.Add(models.ToArray());
                    return Json(ResponseData.Instance.GenericResponse(response));
                }
                else
                {
                    return Json("Please select Test Name to proceed.");
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

        public async Task<IActionResult> GetTestDetail()
        {
            try
            {
                List<TestQuestionVm> model = new List<TestQuestionVm>();

                model = (from TM in await _ITestQuestionRepo.GetList(x => x.IsActive == 1)
                         join QM in await _IQuestionRepo.GetList(x => x.IsActive == 1)
                         on TM.QuestionId equals QM.Id
                         join TTM in await _ITestRepo.GetList(x => x.IsActive == 1)
                         on TM.TestId equals TTM.Id
                         select new TestQuestionVm
                         {
                             TestId = TTM.Id,
                             TestName = TTM.TestName,
                             Question = QM.Question,
                             QuestionId = QM.Id,
                             QuestionMark = QM.QuestionPoint,
                             TimeLimit = TTM.TestTimeLimit
                         }).ToList();

                return PartialView("~/Views/OnlineTest/TestQuestionMappingPartial.cshtml", model);
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

        public async Task<IActionResult> PublishTest(string dateTime, int testId, int templateId)
        {
            try
            {
                string[] DateTime = dateTime.Split('T');
                DateTime publishDate = Convert.ToDateTime(DateTime[0]);
                TimeSpan publishTime = TimeSpan.Parse(DateTime[1]);

                var model = await _ITestRepo.GetSingle(x => x.Id == testId);
                model.TestDateTime = publishDate + publishTime;
                await _ITestRepo.CreateNewContext();
                var response = await _ITestRepo.Update(model);

                var notificationTemplate = await _smsTemplateRepo.GetSingle(x => x.Id == templateId);

                await _ITestRepo.CreateNewContext();

                var testModel = await _ITestRepo.GetSingle(x => x.Id == testId);

                var studentIds = (await _studentPromoteRepo.GetList(x => x.CourseId == testModel.CourseId && x.BatchId == testModel.BatchId)).ToList();

                List<Tuple<string, string>> studentinfos = new List<Tuple<string, string>>();

                var studentModels = await _studentMasterRepo.GetList(x => x.IsActive == 1);

                studentIds.ToList().ForEach(x =>
                {
                    var student = studentModels.ToList().Find(z => z.Id == x.StudentId);
                    studentinfos.Add(new Tuple<string, string>(student.Name, student.StudentEmail));
                });

                var mailResponse = new SendEmailNotification().SendOnlineTestEmail(studentinfos, new Tuple<string, string>("VipraAdmin", "Bhaweshdeepak@gmail.com"), "Exam Notification", _hostingEnviroment);
                if (mailResponse.Count() > 0)
                {
                    return Json(mailResponse);
                }

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

        public async Task<IActionResult> GetOptionsList(int questionId)
        {
            try
            {
                var optionModel = await _optionMasterrepo.GetList(x => x.IsActive == 1 && x.QuestionId == questionId);
                return PartialView("~/Views/OnlineTest/_QuestionOptionDetailPartial.cshtml", optionModel);
            }
            catch(Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }
    }
}