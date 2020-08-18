using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.HRModule
{
    public class PayHeadController : Controller
    {
        private readonly IGenericRepository<PayHeadesModel, int> _payHeadRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public PayHeadController(IGenericRepository<PayHeadesModel, int> payHeadRepo,
                                 IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _payHeadRepo = payHeadRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                if (id == 0)
                {
                    return await Task.Run(() => PartialView("~/Views/HRModule/_PayHeadCreatePartial.cshtml"));
                }
                else
                {
                    var result = await _payHeadRepo.GetSingle(x => x.Id == id);
                    return await Task.Run(() => PartialView("~/Views/HRModule/_PayHeadCreatePartial.cshtml", result));
                }
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
        public async Task<IActionResult> CreatePayHead(PayHeadesModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now.Date;
                    var result = await _payHeadRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    var result = await _payHeadRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }

        public async Task<IActionResult> GetPayHeadDetail()
        {
            try
            {
                var result = await _payHeadRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/HRModule/_PayHeadListPartial.cshtml", result);
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

        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                var payHeadModel = await _payHeadRepo.GetSingle(x => x.Id == id);
                payHeadModel.IsActive = 0;
                payHeadModel.IsDeleted = 1;

                await _payHeadRepo.CreateNewContext();
                var result = await _payHeadRepo.Delete(payHeadModel);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }
    }
}