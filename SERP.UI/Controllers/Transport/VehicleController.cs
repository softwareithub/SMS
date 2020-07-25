using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Transport;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transport
{
    public class VehicleController : Controller
    {
        private readonly IGenericRepository<VehicleModel, int> _vehicleRepo;

        public VehicleController(IGenericRepository<VehicleModel, int> vehicleRepo)
        {
            _vehicleRepo = vehicleRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var responseData = await _vehicleRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/Transport/_VehicleMasterPartial.cshtml",responseData);
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
            var response = await _vehicleRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/Transport/_VehicleDetailPartial.cshtml", response);
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