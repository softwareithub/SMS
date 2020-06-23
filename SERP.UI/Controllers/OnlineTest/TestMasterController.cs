using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SERP.Core.Entities.OnlineTest;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.OnlineTest
{
    public class TestMasterController : Controller
    {
        private readonly IGenericRepository<TestMaster, int> _TestMasterRepo;
        public TestMasterController(IGenericRepository<TestMaster, int> testMasterRepo)
        {
            _TestMasterRepo = testMasterRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var model = await _TestMasterRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/OnlineTest/_TestMasterPartial.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTest(TestMaster model)
        {
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
            var response = await _TestMasterRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/OnlineTest/_TestDetailPartial.cshtml", response);
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