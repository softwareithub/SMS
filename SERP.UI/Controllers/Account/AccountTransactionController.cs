using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Accounts;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Account
{
    public class AccountTransactionController : Controller
    {
        private readonly IGenericRepository<AccountTransaction, int> _accountTransactionRepo;
        private readonly IGenericRepository<ExpenseHead, int> _expenseHeadRepo;
        private readonly IGenericRepository<IncomeHeads, int> _incomeHeadRepo;
        private readonly IGenericRepository<AccountDetail, int> _accountRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IAccountTransactionRepo _accountTransactionDetailRepo;

        public AccountTransactionController(IGenericRepository<AccountTransaction, int> accountTransactionRepo, IGenericRepository<ExpenseHead, int> expenseRepo, IGenericRepository<IncomeHeads, int> incomeRepo, IGenericRepository<AccountDetail, int> accountRepo, IGenericRepository<StudentMaster, int> studentRepo, IAccountTransactionRepo accountTransactionDetailRepo)
        {
            _accountTransactionRepo = accountTransactionRepo;
            _expenseHeadRepo = expenseRepo;
            _incomeHeadRepo = incomeRepo;
            _accountRepo = accountRepo;
            _studentRepo = studentRepo;
            _accountTransactionDetailRepo = accountTransactionDetailRepo;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.ExpenseDetails = await _expenseHeadRepo.GetList(x => x.IsActive == 1);
            ViewBag.IncomeDetails = await _incomeHeadRepo.GetList(x => x.IsActive == 1);
            ViewBag.AccountDetails = await _accountRepo.GetList(x => x.IsActive == 1);
            ViewBag.StudentList = await _studentRepo.GetList(x => x.IsActive == 1);

            var model = await _accountTransactionRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/Accounts/_AccountTransactionCreatePartial.cshtml",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountTransaction model)
        {
            if (model.Id > 0)
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<AccountTransaction>(model, 1);
                return Json(ResponseData.Instance.GenericResponse(await _accountTransactionRepo.Update(updateModel)));
            }
            else
            {
                return Json(ResponseData.Instance.GenericResponse(await _accountTransactionRepo.CreateEntity(model)));
            }
            
        }

        public async Task<IActionResult> GetList()
        {
          
            return PartialView("~/Views/Accounts/_AccountTrancationModelDetailPartial.cshtml", await _accountTransactionDetailRepo.GetAccountTransactionDetails());
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _accountTransactionRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<AccountTransaction>(model,1);
            await _accountTransactionRepo.CreateNewContext();
            return Json(await _accountTransactionRepo.Update(deleteModel));

        }

        public async Task<IActionResult> AccountSummary()
        {
            return PartialView("~/Views/Accounts/_AccountSummaryDetailPartial.cshtml",await _accountTransactionDetailRepo.GetAccountTransactionSummaryDetails());
        }

    }
}