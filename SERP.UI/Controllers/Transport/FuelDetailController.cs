using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Transport;
using SERP.Core.Model.Transport;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transport
{
    public class FuelDetailController : Controller
    {
        private readonly IGenericRepository<VehicleFuelDetail, int> _vehicleFuelRepo;
        private readonly IGenericRepository<VehicleModel, int> _vehicalRepo;
        private readonly IHostingEnvironment _hostingEnviroment;

        public FuelDetailController(IGenericRepository<VehicleFuelDetail, int> vehicleFuelRepo, IGenericRepository<VehicleModel, int> vehicleRepo, IHostingEnvironment hostingEnvironment)
        {
            _vehicleFuelRepo = vehicleFuelRepo;
            _vehicalRepo = vehicleRepo;
            _hostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Vehicles = await _vehicalRepo.GetList(x => x.IsActive == 1);
            var model = await _vehicleFuelRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/Transport/_VehicleFuelDetailPartial.cshtml", model);
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
            var models = (from FR in await _vehicleFuelRepo.GetList(x => x.IsActive == 1)
                          join VD in await _vehicalRepo.GetList(x => x.IsActive == 1)
                          on FR.VehicleId equals VD.Id
                          select new VehicleFuelModel
                          {
                              Id= FR.Id,
                              VehicleName= VD.VehicleName,
                              VehicleNumber= VD.VehicleNumber,
                              FuelDate= FR.FuelDate,
                              Quantity= FR.Quantity,
                              Rate= FR.Rate,
                              RecieptImage= FR.RecieptImage,
                              ReciptNumber= FR.ReciptNumber,
                              Remark= FR.Remark
                          }).ToList();

            return PartialView("~/Views/Transport/_FuelDetailPartial.cshtml", models);
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