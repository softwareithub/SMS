using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LibraryManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.LibraryManagement
{
    public class LibrarySetingController : Controller
    {
        private readonly IGenericRepository<LibrarySetting, int> _librarySettingRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        public LibrarySetingController(IGenericRepository<LibrarySetting, int> librarySettingRepo)
        {
            _librarySettingRepo = librarySettingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var model =await  _librarySettingRepo.GetSingle(x=>x.Id==id);
            return PartialView("~/Views/LibraryManagement/Setting/_LibrarySettingPartial.cshtml",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LibrarySetting model)
        {
            if (model.Id > 0)
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<LibrarySetting>(model, 1);
                var response = await _librarySettingRepo.Update(updateModel);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else {
                return Json(ResponseData.Instance.GenericResponse(await _librarySettingRepo.CreateEntity(model)));
            }
        }

        public async Task<IActionResult> GetDetails()
        {
            var response = await _librarySettingRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/LibraryManagement/Setting/_LibrarySettingDetailPartial.cshtml", response);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _librarySettingRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<LibrarySetting>(model, 1);
            await _librarySettingRepo.CreateNewContext();
            return Json(ResponseData.Instance.GenericResponse(await _librarySettingRepo.Update(deleteModel)));
        }
    }
}