using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Accounts;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Account
{
    public class ExpenseHeadController : Controller
    {
        private readonly IGenericRepository<ExpenseHead, int> _expenseHeadRepo;
        public ExpenseHeadController(IGenericRepository<ExpenseHead, int> expenseHeadRepo)
        {
            _expenseHeadRepo = expenseHeadRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var model = await _expenseHeadRepo.GetSingle(x => x.IsActive == 1);
            return PartialView("~/Views/Accounts/_CreateExpenseHeadPartial.cshtml",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseHead model)
        {
            if(model.Id>0)
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<ExpenseHead>(model, 1);
                return Json(ResponseData.Instance.GenericResponse(await _expenseHeadRepo.Update(updateModel)));
            }
            return Json(ResponseData.Instance.GenericResponse(await _expenseHeadRepo.CreateEntity(model)));
        }

        public async Task<IActionResult> GetList()
        {
            return PartialView("~/Views/Accounts/_ExpenseHeadListPartial.cshtml",await _expenseHeadRepo.GetList(x => x.IsActive == 1));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _expenseHeadRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<ExpenseHead>(model, 1);
            await _expenseHeadRepo.CreateNewContext();
            return Json(ResponseData.Instance.GenericResponse(await _expenseHeadRepo.Update(deleteModel)));
        }
    }
}