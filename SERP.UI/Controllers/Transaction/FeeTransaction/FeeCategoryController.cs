using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transaction.FeeTransaction
{
    public class FeeCategoryController : Controller
    {
        private readonly IGenericRepository<FeeCategory, int> _feeCategoryRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public FeeCategoryController(IGenericRepository<FeeCategory, int> feeCategoryRepo,
                    IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _feeCategoryRepo = feeCategoryRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id = 0)
        {
            try
            {
                if (id == 0)
                    return await Task.Run(() => PartialView("~/Views/FeeTransaction/_FeeCategoryPartial.cshtml"));

                var model = await _feeCategoryRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/FeeTransaction/_FeeCategoryPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateFeeCategory(FeeCategory model)
        {
            if (model.Id == 0)
            {
                model.CreatedBy = 1;
                var result = await _feeCategoryRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }

            else
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var result = await _feeCategoryRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
        }

        public async Task<IActionResult> GetFeeCategoryList()
        {
            try
            {
                var result = await _feeCategoryRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/FeeTransaction/_FeeCategoryListPartial.cshtml", result);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> DeleteFeeCategory(int id)
        {
            var model = await _feeCategoryRepo.GetSingle(x => x.Id == id);
            model.IsDeleted = 1;
            model.IsActive = 0;
            model.UpdatedBy = 1;
            model.UpdatedDate = DateTime.Now.Date;

            await _feeCategoryRepo.CreateNewContext();
            var result = await _feeCategoryRepo.Update(model);

            return Json(ResponseData.Instance.GenericResponse(result));
        }
    }
}