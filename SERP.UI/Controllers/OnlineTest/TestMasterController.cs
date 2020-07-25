using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.OnlineTest;
using SERP.Core.Model.OnlineTest;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.OnlineTest
{
    public class TestMasterController : Controller
    {
        private readonly IGenericRepository<TestMaster, int> _TestMasterRepo;
        private readonly IGenericRepository<CourseMaster, int> _CourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _batchRepo;
        public TestMasterController(IGenericRepository<TestMaster, int> testMasterRepo, IGenericRepository<CourseMaster, int>  courseRepo, IGenericRepository<BatchMaster, int> batchRepo)
        {
            _TestMasterRepo = testMasterRepo;
            _CourseRepo = courseRepo;
            _batchRepo = batchRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.CourseList = await _CourseRepo.GetList(x => x.IsActive == 1);
            var model = await _TestMasterRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/OnlineTest/_TestMasterPartial.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTest(TestMaster model)
        {
            model.TestDateTime = DateTime.Now;
            if (model.Id > 0)
            {
                var response = await _TestMasterRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var response = await _TestMasterRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }
        public async Task<IActionResult> GetTestDetails()
        {
            var model = (from TM in await _TestMasterRepo.GetList(x => x.IsActive == 1)
                        join CM in await _CourseRepo.GetList(x => x.IsActive == 1)
                        on TM.CourseId equals CM.Id
                        join BM in await _batchRepo.GetList(x => x.IsActive == 1)
                        on TM.BatchId equals BM.Id
                        select new TestMasterVm
                        { 
                            TestId= TM.Id,
                            TestName= TM.TestName,
                            TestRuleRegulation= TM.Regulation,
                            TestTimeLimit= TM.TestTimeLimit,
                            CourseName=CM.Name,
                            BatchName= BM.BatchName
                        
                        }).ToList();

            return PartialView("~/Views/OnlineTest/_TestDetailPartial.cshtml", model);
        }

        public async Task<IActionResult> DeleteTest(int id)
        {
            var model = await _TestMasterRepo.GetSingle(x => x.Id == id);
            model.IsActive = 0;
            model.IsDeleted = 1;
            await _TestMasterRepo.CreateNewContext();
            var response = await _TestMasterRepo.Update(model);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}