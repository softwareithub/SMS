using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Master
{
    public class InstituteSettingController : Controller
    {
        private readonly IGenericRepository<InstituteSettingModel, int> _instituteRepo;
        private readonly IGenericRepository<InstituteMaster, int> _instituteMasterRepo;

        public InstituteSettingController(IGenericRepository<InstituteSettingModel, int> instituteRepo, IGenericRepository<InstituteMaster, int> instituteMasterRepo)
        {
            _instituteRepo = instituteRepo;
            _instituteMasterRepo = instituteMasterRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var instituteModel = (await _instituteMasterRepo.GetSingle(x => x.IsActive == 1)).Id;
            var model = await _instituteRepo.GetSingle(x => x.InstituteId == instituteModel);
            return PartialView("~/Views/InstituteMaster/_InstituteSettingPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstituteSettingModel model)
        {
            var instituteModel = (await _instituteMasterRepo.GetSingle(x => x.IsActive == 1)).Id;
            model.InstituteId = instituteModel;
            if (model.Id == 0)
            {
                var response = await _instituteRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode(model, 1);
                var response = await _instituteRepo.Update(updateModel);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }
    }
}