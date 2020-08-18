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
    public class ReligionController : Controller
    {
        private readonly IGenericRepository<ReligionMaster, int> _IReligionMaster;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public ReligionController(IGenericRepository<ReligionMaster, int> religionMaster,
                                  IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IReligionMaster = religionMaster;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> CreateReligion(int id=0)
        {
            try
            {
                if (id == 0)
                    return await Task.Run(() => PartialView("~/Views/ReligionMaster/_CreateReligionPartial.cshtml"));

                var modal = await _IReligionMaster.GetSingle(x => x.Id == id);
                return PartialView("~/Views/ReligionMaster/_CreateReligionPartial.cshtml", modal);
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
        public async Task<IActionResult> Create(ReligionMaster modal)
        {
            try
            {
                if (modal.Id == 0)
                {
                    var result = await _IReligionMaster.CreateEntity(modal);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    var result = await _IReligionMaster.Update(modal);
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
        public async Task<IActionResult> GetReligionMaster()
        {
            try
            {
                var result = await _IReligionMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/ReligionMaster/_ReligionListPartial.cshtml", result);
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

        public async Task<IActionResult>DeleteReligion(int id)
        {
            try
            {
                var model = await _IReligionMaster.GetSingle(x => x.Id == id);
                await _IReligionMaster.CreateNewContext();
                model.IsActive = 0; model.IsDeleted = 1;
                model.UpdatedBy = 1; model.UpdatedDate = DateTime.Now.Date;
                var result = await _IReligionMaster.Delete(model);
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