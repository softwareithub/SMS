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
    public class DesignationController : Controller
    {
        private readonly IGenericRepository<DesignationModel, int> _designationRepo;
        public DesignationController(IGenericRepository<DesignationModel, int> deignationRepo)
        {
            _designationRepo = deignationRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var result = await _designationRepo.GetSingle(x => x.Id == id);
            return View("~/Views/HRModule/_DesignationCreatePartial.cshtml", result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(DesignationModel model)
        {
            if (model.Id == 0)
            {
                var result = await _designationRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var result = await _designationRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
        }

        public async Task<IActionResult> GetDesignation()
        {
            var result = await _designationRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/HRModule/_DesignationListPartial.cshtml", result);
        }

        public async Task<IActionResult> DeleteRecord(int id)
        {
            var model = await _designationRepo.GetSingle(x => x.Id == id);
            model.IsActive = 0;
            model.IsDeleted = 1;

            await _designationRepo.CreateNewContext();

            var result = await _designationRepo.Delete(model);
            return Json(ResponseData.Instance.GenericResponse(result));
        }

    }
}