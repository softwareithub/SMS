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
    public class AccountDetailController : Controller
    {
        private readonly IGenericRepository<AccountDetail, int> _accountRepo;

        public AccountDetailController(IGenericRepository<AccountDetail, int> accountRepo)
        {
            _accountRepo = accountRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var model =await _accountRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/Accounts/_CreateAccountPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountDetail model)
        {
            if(model.Id>0)
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<AccountDetail>(model, 1);
                return Json(ResponseData.Instance.GenericResponse(await _accountRepo.Update(updateModel)));
            }
            return Json(ResponseData.Instance.GenericResponse(await _accountRepo.CreateEntity(model)));
        }

        public async Task<IActionResult> GetList()
        {
            return PartialView("~/Views/Accounts/_AccountDetailPartial.cshtml", await _accountRepo.GetList(x => x.IsActive == 1));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _accountRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<AccountDetail>(model, 1);
            await _accountRepo.CreateNewContext();
            return Json(ResponseData.Instance.GenericResponse(await _accountRepo.Update(deleteModel)));
        }

    }
}