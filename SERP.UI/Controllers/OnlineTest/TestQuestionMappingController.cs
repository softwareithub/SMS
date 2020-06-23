using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.OnlineTest;
using SERP.Core.Model.ExamModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.OnlineTest
{
    public class TestQuestionMappingController : Controller
    {
        private readonly IGenericRepository<QuestionModel, int> _IQuestionRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectMasterRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<TestMaster, int> _ITestRepo;
        private readonly IGenericRepository<TestQuestionMapping, int> _ITestQuestionRepo;

        public TestQuestionMappingController(IGenericRepository<QuestionModel, int> questionRepo, IGenericRepository<SubjectMaster, int> subjectReo, IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<TestMaster, int> testRepo, IGenericRepository<TestQuestionMapping, int> testQuestionRepo)
        {
            _IQuestionRepo = questionRepo;
            _ISubjectMasterRepo = subjectReo;
            _ICourseRepo = courseRepo;
            _ITestRepo = testRepo;
            _ITestQuestionRepo = testQuestionRepo;
        }
        public async Task<IActionResult> Index()
        {
            var courseDetails = await _ICourseRepo.GetList(x => x.IsActive == 1);
            var subjectDetails = await _ISubjectMasterRepo.GetList(x => x.IsActive == 1);
            var testDetails = await _ITestRepo.GetList(x => x.IsActive == 1);
            var questionDetails = await _IQuestionRepo.GetList(x => x.IsActive == 1);

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
                                QuestionPoint = QM.QuestionPoint

                            }).ToList();

            ViewBag.CourseDetail = courseDetails;

            ViewBag.TestDetail = testDetails;

            return PartialView("~/Views/OnlineTest/_TestQuestionMappingPartial.cshtml", response);
        }

        public async Task<IActionResult> GetSubjectDetail(int courseId)
        {
            var responseData = await _ISubjectMasterRepo.GetList(x => x.CourseId == courseId && x.IsActive == 1);
            return Json(responseData);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(int TestId, int[] QuestionId,int questionMark)
        {
            if (TestId != 0)
            {
                List<TestQuestionMapping> models = new List<TestQuestionMapping>();
                for (int i = 0; i < QuestionId.Count(); i++)
                {
                    TestQuestionMapping model = new TestQuestionMapping();
                    model.QuestionId = QuestionId[i];
                    model.TestId = TestId;
                    models.Add(model);
                }
                var response=await _ITestQuestionRepo.Add(models.ToArray());
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                return Json("Please select Test Name to proceed.");
            }
        }

      
    }
}