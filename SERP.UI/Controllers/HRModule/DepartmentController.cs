using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class DepartmentController : Controller
    {
        private readonly IGenericRepository<DepartmentModel, int> _departmentRepo;
        public DepartmentController(IGenericRepository<DepartmentModel, int> departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var result = await _departmentRepo.GetSingle(x => x.Id == id);
            return await Task.Run(() => PartialView("~/Views/HRModule/_DepartmentCreatePartial.cshtml",result));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(DepartmentModel model)
        {
            if (model.Id == 0)
            {
                var result = await _departmentRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var result = await _departmentRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
        }

        public async Task<IActionResult> GetDepartmentList()
        {
            var result = await _departmentRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/HRModule/_DepartmentListPartial.cshtml", result);
        }

        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var departmentModel = await _departmentRepo.GetSingle(x => x.Id == id);
            departmentModel.IsActive = 0;
            departmentModel.IsDeleted = 1;
            departmentModel.UpdatedBy = 1;
            departmentModel.UpdatedDate = DateTime.Now.Date;

            await _departmentRepo.CreateNewContext();

            var result = await _departmentRepo.Delete(departmentModel);

            return Json(ResponseData.Instance.GenericResponse(result));
        }
    }
}