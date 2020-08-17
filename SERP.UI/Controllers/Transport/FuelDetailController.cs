using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Entities.Transport;
using SERP.Core.Model.Transport;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transport
{
    public class FuelDetailController : Controller
    {
        private readonly IGenericRepository<VehicleFuelDetail, int> _vehicleFuelRepo;
        private readonly IGenericRepository<VehicleModel, int> _vehicalRepo;
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        

        public FuelDetailController(IGenericRepository<VehicleFuelDetail, int> vehicleFuelRepo, IGenericRepository<VehicleModel, int> vehicleRepo, IHostingEnvironment hostingEnvironment, IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _vehicleFuelRepo = vehicleFuelRepo;
            _vehicalRepo = vehicleRepo;
            _hostingEnviroment = hostingEnvironment;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.Vehicles = await _vehicalRepo.GetList(x => x.IsActive == 1);
                var model = await _vehicleFuelRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/Transport/_VehicleFuelDetailPartial.cshtml", model);
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
        public async Task<IActionResult> Create(VehicleFuelDetail model, IFormFile RecieptImage)
        {
            if(RecieptImage!=null && RecieptImage.Length>0)
            {
                var upload = Path.Combine(_hostingEnviroment.WebRootPath, "Images//");
                using (FileStream fs = new FileStream(Path.Combine(upload, RecieptImage.FileName), FileMode.Create))
                {
                    await RecieptImage.CopyToAsync(fs);
                }
                model.RecieptImage = "/Images/" + RecieptImage.FileName;
            }
            if (model.Id == 0)
            {
                var response = await _vehicleFuelRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<VehicleFuelDetail>(model, 1);
                return Json(ResponseData.Instance.GenericResponse(await _vehicleFuelRepo.Update(updateModel)));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var models = (from FR in await _vehicleFuelRepo.GetList(x => x.IsActive == 1)
                              join VD in await _vehicalRepo.GetList(x => x.IsActive == 1)
                              on FR.VehicleId equals VD.Id
                              select new VehicleFuelModel
                              {
                                  Id = FR.Id,
                                  VehicleName = VD.VehicleName,
                                  VehicleNumber = VD.VehicleNumber,
                                  FuelDate = FR.FuelDate,
                                  Quantity = FR.Quantity,
                                  Rate = FR.Rate,
                                  RecieptImage = FR.RecieptImage,
                                  ReciptNumber = FR.ReciptNumber,
                                  Remark = FR.Remark
                              }).ToList();

                return PartialView("~/Views/Transport/_FuelDetailPartial.cshtml", models);
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
            var model = await _vehicleFuelRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<VehicleFuelDetail>(model, 1);
            await _vehicleFuelRepo.CreateNewContext();
            return Json(ResponseData.Instance.GenericResponse(await _vehicleFuelRepo.Update(deleteModel)));
        }
    }
}