using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Master
{
    public class CasteMasterController : Controller
    {
        private readonly IGenericRepository<CasteMaster, int> _ICasteRespo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public CasteMasterController(IGenericRepository<CasteMaster, int> casteRepo,
                                     IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _ICasteRespo = casteRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> CasteMaster(int id=0)
        {
            try
            {
                if (id == 0)
                    return await Task.Run(() => PartialView("~/Views/CasteMaster/_CasteMasterPartial.cshtml"));

                return PartialView("~/Views/CasteMaster/_CasteMasterPartial.cshtml", await _ICasteRespo.GetSingle(x => x.Id == id));
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
        public async Task<IActionResult> CreateCaste(CasteMaster model)
        {
            try
            {
                model.IsActive = 1; model.IsDeleted = 0;
                var result = model.Id == 0 ? await InsertCaste(model) : await UpdateCaste(model);
                return Json(result);
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

        public async Task<IActionResult> GetCasteList()
        {
            try
            {
                var result = await _ICasteRespo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/CasteMaster/_CasteMasterList.cshtml", result);
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

        public async Task<IActionResult> DeleteCaste(int id)
        {
            try
            {
                var model = await _ICasteRespo.GetSingle(x => x.Id == id);
                await _ICasteRespo.CreateNewContext();
                model.IsActive = 0; model.IsDeleted = 1; model.UpdatedBy = 1; model.UpdatedDate = DateTime.Now.Date;
                var deleteResult = await _ICasteRespo.Delete(model);
                return Json(ResponseData.Instance.GenericResponse(deleteResult));
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

        #region Private
        public async Task<string> InsertCaste(CasteMaster model)
        {
            model.CreatedBy = 1;model.CreatedDate = DateTime.Now.Date;
            var result =await _ICasteRespo.CreateEntity(model);
            return ResponseData.Instance.GenericResponse(result);
        }
        public async Task<string> UpdateCaste(CasteMaster model)
        {
            model.UpdatedBy = 1; model.UpdatedDate = DateTime.Now.Date;
            var result = await _ICasteRespo.Update(model);
            return ResponseData.Instance.GenericResponse(result);
        }
        #endregion

    }
}