using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Accounts;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;
using System.Threading.Tasks;

namespace SERP.UI.Controllers.Account
{
    public class IncomeHeadController : Controller
    {
        private readonly IGenericRepository<IncomeHeads, int> _IncomeHeadRepo;

        public IncomeHeadController(IGenericRepository<IncomeHeads, int> incomeHeadRepo)
        {
            _IncomeHeadRepo = incomeHeadRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var model = await _IncomeHeadRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/Accounts/_CreateIncomeHeadPartial.cshtml",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IncomeHeads model)
        {
            if(model.Id>0)
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<IncomeHeads>(model, 1);
                return Json(ResponseData.Instance.GenericResponse(await _IncomeHeadRepo.Update(updateModel)));
            }
            return Json(ResponseData.Instance.GenericResponse(await _IncomeHeadRepo.CreateEntity(model)));
        }

        public async  Task<IActionResult> GetList()
        {
            var models = await _IncomeHeadRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/Accounts/_IncomeHeadListPartial.cshtml", models);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _IncomeHeadRepo.GetSingle(x => x.IsActive == 1 && x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<IncomeHeads>(model, 1);
            await _IncomeHeadRepo.CreateNewContext();
            return Json(ResponseData.Instance.GenericResponse(await _IncomeHeadRepo.Update(deleteModel)));
        }
    }
}