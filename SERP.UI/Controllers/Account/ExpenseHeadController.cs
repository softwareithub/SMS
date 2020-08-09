using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ExpenseHeadController : Controller
    {
        private readonly IGenericRepository<ExpenseHead, int> _expenseHeadRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public ExpenseHeadController(IGenericRepository<ExpenseHead, int> expenseHeadRepo, IGenericRepository<ExceptionLogging, int> exceptionRepo)
        {
            _expenseHeadRepo = expenseHeadRepo;
            _exceptionLoggingRepo = exceptionRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var model = await _expenseHeadRepo.GetSingle(x => x.IsActive == 1);
                return PartialView("~/Views/Accounts/_CreateExpenseHeadPartial.cshtml", model);
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Index), nameof(ExpenseHead), ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));

            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseHead model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var updateModel = CommanDeleteHelper.CommanUpdateCode<ExpenseHead>(model, 1);
                    return Json(ResponseData.Instance.GenericResponse(await _expenseHeadRepo.Update(updateModel)));
                }
                return Json(ResponseData.Instance.GenericResponse(await _expenseHeadRepo.CreateEntity(model)));
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Create), nameof(ExpenseHead), ex.Message, LoggingType.httpPost.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }

        }

        public async Task<IActionResult> GetList()
        {
            
            try
            {
                return PartialView("~/Views/Accounts/_ExpenseHeadListPartial.cshtml", await _expenseHeadRepo.GetList(x => x.IsActive == 1));
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(GetList), nameof(ExpenseHead), ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));

            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _expenseHeadRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<ExpenseHead>(model, 1);
                await _expenseHeadRepo.CreateNewContext();
                return Json(ResponseData.Instance.GenericResponse(await _expenseHeadRepo.Update(deleteModel)));
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Delete), nameof(ExpenseHead), ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }

        }
    }
}