using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.UserManagement
{
    public class MenuController : Controller
    {
        private readonly IGenericRepository<ModuleMaster, int> _IModuleRepo;

        public MenuController(IGenericRepository<ModuleMaster, int> _moduleRepo)
        {
            _IModuleRepo = _moduleRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            return PartialView("~/Views/UserManagement/_ModuleMasterView.cshtml",await _IModuleRepo.GetSingle(x=>x.Id==id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu(ModuleMaster model)
        {
            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var response = await _IModuleRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else {
                var response = await _IModuleRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }

        public async Task<IActionResult> ModuleList()
        {
            var responseData = await _IModuleRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/UserManagement/_ModuleMasterListPartial.cshtml", responseData);
        }

        public async Task<IActionResult> DeleteModule(int id)
        {
            var responseData = await _IModuleRepo.GetSingle(x => x.Id == id);
            var deleteModel =CommanDeleteHelper.CommanDeleteCode<ModuleMaster>(responseData,1);
            await _IModuleRepo.CreateNewContext();
            var response = await _IModuleRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}