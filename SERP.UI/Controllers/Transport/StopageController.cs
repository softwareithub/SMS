using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Transport;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transport
{
    public class StopageController : Controller
    {
        private readonly IGenericRepository<StopageModel, int> _stopageRepository;
        public StopageController(IGenericRepository<StopageModel, int> stopageRepository)
        {
            _stopageRepository = stopageRepository;
        }
        public async Task<IActionResult> Index(int id)
        {
            var responseData = await _stopageRepository.GetSingle(x => x.Id == id);
            return PartialView("~/Views/Transport/_StopagePartial.cshtml",responseData);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStopage(StopageModel model)
        {
            var response = await _stopageRepository.CreateEntity(model);
            return Json(ResponseData.Instance.GenericResponse(response));
        }

        public async Task<IActionResult> GetStopageDetails()
        {
            var responseData = await _stopageRepository.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/Transport/_StopageDetailPartial.cshtml", responseData);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _stopageRepository.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<StopageModel>(model, 1);
            await _stopageRepository.CreateNewContext();
            var response = await _stopageRepository.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}