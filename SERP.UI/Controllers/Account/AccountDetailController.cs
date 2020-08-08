using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Accounts;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Account
{
    public class AccountDetailController : Controller
    {
        private readonly IGenericRepository<AccountDetail, int> _accountRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public AccountDetailController(IGenericRepository<AccountDetail, int> accountRepo, IGenericRepository<ExceptionLogging, int> exceptionRepo)
        {
            _accountRepo = accountRepo;
            _exceptionLoggingRepo = exceptionRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var model = await _accountRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/Accounts/_CreateAccountPartial.cshtml", model);
            }
            catch (Exception ex)
            {

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("Index", "AccountDetail", ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountDetail model)
        {

            try
            {
                if (model.Id > 0)
                {
                    var updateModel = CommanDeleteHelper.CommanUpdateCode<AccountDetail>(model, 1);
                    return Json(ResponseData.Instance.GenericResponse(await _accountRepo.Update(updateModel)));
                }
                return Json(ResponseData.Instance.GenericResponse(await _accountRepo.CreateEntity(model)));
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("Create", "AccountDetail", ex.Message, LoggingType.httpPost.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }

        }

        public async Task<IActionResult> GetList()
        {
            try
            {
                return PartialView("~/Views/Accounts/_AccountDetailPartial.cshtml", await _accountRepo.GetList(x => x.IsActive == 1));
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj("GetList", "AccountDetail", ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _accountRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<AccountDetail>(model, 1);
                await _accountRepo.CreateNewContext();
                return Json(ResponseData.Instance.GenericResponse(await _accountRepo.Update(deleteModel)));
            }
            catch (Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Delete), nameof(AccountDetail), ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }

        }

    }
}