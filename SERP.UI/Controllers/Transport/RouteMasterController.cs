using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Transport;
using SERP.Core.Model.Transport;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transport
{
    public class RouteMasterController : Controller
    {
        private readonly IGenericRepository<RouteMaster, int> _routeMaster;
        private readonly IGenericRepository<RouteStopageModel, int> _routeStopageRepo;
        private readonly IGenericRepository<VehicleModel, int> _vehicleRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<StopageModel, int> _stopageRepo;

        public RouteMasterController(IGenericRepository<RouteMaster, int> routeRepo, IGenericRepository<RouteStopageModel, int> routeStopageRepo, IGenericRepository<VehicleModel, int> vehicleRepo, IGenericRepository<EmployeeBasicInfoModel, int> empRepo, IGenericRepository<StopageModel, int> stopageRepo)
        {
            _routeMaster = routeRepo;
            _routeStopageRepo = routeStopageRepo;
            _employeeRepo = empRepo;
            _vehicleRepo = vehicleRepo;
            _stopageRepo = stopageRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Employees = await _employeeRepo.GetList(x => x.IsActive == 1);
            ViewBag.Vehicles = await _vehicleRepo.GetList(x => x.IsActive == 1);

            return await Task.Run(() => PartialView("~/Views/Transport/_RouteMasterPartial.cshtml"));
        }

        public async Task<IActionResult> AddStopage()
        {
            return PartialView("~/Views/Transport/AddStopageToRoutePartial.cshtml", await _stopageRepo.GetList(x => x.IsActive == 1));
        }

        [HttpPost]
        public async Task<IActionResult> Create(RouteViewModel model, string[] stopageIds, string[] stopages)
        {
            var createRouteResponse = await _routeMaster.CreateEntity(model.RouteMaster);
            if (createRouteResponse == Utilities.ResponseUtilities.ResponseStatus.AddedSuccessfully)
            {
                await _routeMaster.CreateNewContext();
                var maxRouteId = (await _routeMaster.GetList(x => x.IsActive == 1)).Max(x => x.Id);

                List<RouteStopageModel> rsModels = new List<RouteStopageModel>();

                for (int i = 0; i < stopageIds.Count(); i++)
                {
                    RouteStopageModel rsmodel = new RouteStopageModel()
                    {
                        RouteId = maxRouteId,
                        StopageId = Convert.ToInt32(stopageIds[i]),
                        RouteType = stopages[i]
                    };
                    rsModels.Add(rsmodel);
                }

                var response = await _routeStopageRepo.Add(rsModels.ToArray());
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            return Json("Error please contact admin!");
        }

        public async Task<IActionResult> GetRouteDetails()
        {
            List<RouteDetailVm> models = new List<RouteDetailVm>();
            var routeModels = await _routeMaster.GetList(x => x.IsActive == 1);
            var vehicleModels = await _vehicleRepo.GetList(x => x.IsActive == 1);
            var driverModels = await _employeeRepo.GetList(x => x.IsActive == 1);

            models = (from RM in routeModels
                      join VM in vehicleModels
                      on RM.VehicleId equals VM.Id
                      join DM in driverModels
                      on RM.DriverId equals DM.Id
                      select new RouteDetailVm
                      {
                          RouteId = RM.Id,
                          RouteName = RM.RouteName,
                          VehicleName = VM.VehicleName,
                          DriverName = DM.Name
                      }).ToList();

            return PartialView("~/Views/Transport/_RouteDetailPartial.cshtml", models);
        }

        public async Task<IActionResult> GetRouteStopage(int id)
        {
            List<StopageDetails> models = new List<StopageDetails>();
            var routeModels = await _routeMaster.GetList(x => x.IsActive == 1);
            var routeStopModels = await _routeStopageRepo.GetList(x => x.IsActive == 1);
            var stopageModels = await _stopageRepo.GetList(x => x.IsActive == 1);
            models = (from RSM in routeStopModels
                      join RM in routeModels
                      on RSM.RouteId equals RM.Id
                      join SM in stopageModels
                      on RSM.StopageId equals SM.Id
                      select new StopageDetails
                      {
                          StopageName= SM.StopageName,
                          Address= SM.PLaceAddress,
                          RouteType=RSM.RouteType

                      }).ToList().OrderBy(x=>x.RouteType).ToList();

            return PartialView("~/Views/Transport/_RouteWiseStopage.cshtml", models);
        }

        public async Task<IActionResult> RouteMapDetails()
        {
            ViewBag.RouteDetails = await _routeMaster.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/Transport/_RouteMapDetailPartial.cshtml");
        }
        public async Task<IActionResult> GetRouteMapDetails(int id)
        {
            List<StopageDetails> models = new List<StopageDetails>();
            var routeModels = await _routeMaster.GetList(x => x.IsActive == 1);
            var routeStopModels = await _routeStopageRepo.GetList(x => x.IsActive == 1 && x.RouteId==id);
            var stopageModels = await _stopageRepo.GetList(x => x.IsActive == 1);
            models = (from RSM in routeStopModels
                      join RM in routeModels
                      on RSM.RouteId equals RM.Id
                      join SM in stopageModels
                      on RSM.StopageId equals SM.Id
                      select new StopageDetails
                      {
                          StopageName = SM.StopageName,
                          Address = SM.PLaceAddress,
                          RouteType = RSM.RouteType

                      }).ToList().OrderBy(x => x.RouteType).ToList();
            return await Task.Run(() => PartialView("~/Views/Transport/_RouteWiseMapDetailPartial.cshtml", models));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _routeStopageRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<RouteStopageModel>(model, 1);
            await _routeStopageRepo.CreateNewContext();
            return Json(ResponseData.Instance.GenericResponse(await _routeStopageRepo.Update(deleteModel)));
        }
    }
}