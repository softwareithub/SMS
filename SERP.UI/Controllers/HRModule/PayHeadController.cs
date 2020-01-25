using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class PayHeadController : Controller
    {
        private readonly IGenericRepository<PayHeadesModel, int> _payHeadRepo;

        public PayHeadController(IGenericRepository<PayHeadesModel, int> payHeadRepo)
        {
            _payHeadRepo = payHeadRepo;
        }
        public async Task<IActionResult> Index(int id)
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
        [HttpPost]
        public async Task<IActionResult> CreatePayHead(PayHeadesModel model)
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

        public async Task<IActionResult> GetPayHeadDetail()
        {
            var result = await _payHeadRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/HRModule/_PayHeadListPartial.cshtml", result);
        }

        public async Task<IActionResult> DeleteRecord(int id)
        {
            var payHeadModel =await _payHeadRepo.GetSingle(x => x.Id == id);
            payHeadModel.IsActive = 0;
            payHeadModel.IsDeleted = 1;

            await _payHeadRepo.CreateNewContext();
            var result = await _payHeadRepo.Delete(payHeadModel);
            return Json(ResponseData.Instance.GenericResponse(result));
        }
    }
}