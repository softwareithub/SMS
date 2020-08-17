using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Entities.Transport;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transport
{
    public class VehicleController : Controller
    {
        private readonly IGenericRepository<VehicleModel, int> _vehicleRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;


        public VehicleController(IGenericRepository<VehicleModel, int> vehicleRepo,
                    IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _vehicleRepo = vehicleRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var responseData = await _vehicleRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/Transport/_VehicleMasterPartial.cshtml", responseData);
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
        public async Task<IActionResult> Create(VehicleModel model)
        {
            if (model.Id == 0)
            {
                var response = await _vehicleRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<VehicleModel>(model, 1);
                var response = await _vehicleRepo.Update(updateModel);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }
        public async Task<IActionResult> GetVehicleList()
        {
            try
            {
                var response = await _vehicleRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/Transport/_VehicleDetailPartial.cshtml", response);
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

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _vehicleRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<VehicleModel>(model, 1);
            await _vehicleRepo.CreateNewContext();
            var response = await _vehicleRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}