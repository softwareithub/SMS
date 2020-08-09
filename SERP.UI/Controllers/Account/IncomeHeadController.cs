using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Accounts;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Threading.Tasks;

namespace SERP.UI.Controllers.Account
{
    public class IncomeHeadController : Controller
    {
        private readonly IGenericRepository<IncomeHeads, int> _IncomeHeadRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public IncomeHeadController(IGenericRepository<IncomeHeads, int> incomeHeadRepo, IGenericRepository<ExceptionLogging, int>  exceptionLogging)
        {
            _IncomeHeadRepo = incomeHeadRepo;
            _exceptionLoggingRepo = exceptionLogging;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var model = await _IncomeHeadRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/Accounts/_CreateIncomeHeadPartial.cshtml", model);
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Index), nameof(IncomeHeads), ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));

            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(IncomeHeads model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var updateModel = CommanDeleteHelper.CommanUpdateCode<IncomeHeads>(model, 1);
                    return Json(ResponseData.Instance.GenericResponse(await _IncomeHeadRepo.Update(updateModel)));
                }
                return Json(ResponseData.Instance.GenericResponse(await _IncomeHeadRepo.CreateEntity(model)));
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Create), nameof(IncomeHeads), ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }

        public async  Task<IActionResult> GetList()
        {
            try
            {
                var models = await _IncomeHeadRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/Accounts/_IncomeHeadListPartial.cshtml", models);
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(GetList), nameof(IncomeHeads), ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));

            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _IncomeHeadRepo.GetSingle(x => x.IsActive == 1 && x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<IncomeHeads>(model, 1);
                await _IncomeHeadRepo.CreateNewContext();
                return Json(ResponseData.Instance.GenericResponse(await _IncomeHeadRepo.Update(deleteModel)));
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Delete), nameof(IncomeHeads), ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
  
        }
    }
}