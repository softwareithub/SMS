using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.LibraryManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.LibraryManagement
{
    public class CategoryController : Controller
    {
        private readonly IGenericRepository<CategoryMaster, int> _categoryRepo;
        public CategoryController(IGenericRepository<CategoryMaster, int> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var response = await _categoryRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/LibraryManagement/Category/_CategoryCreatePartial.cshtml", response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryMaster model)
        {
            if (model.Id == 0)
            {
                var response = await _categoryRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<CategoryMaster>(model,1);
                return Json(ResponseData.Instance.GenericResponse(await _categoryRepo.Update(updateModel)));
            }
        }

        public async Task<IActionResult> GetCategoryList()
        {
            var responseData = await _categoryRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/LibraryManagement/Category/_CategoryListPartial.cshtml", responseData);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _categoryRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<CategoryMaster>(model,1);
            await _categoryRepo.CreateNewContext();
            return Json(ResponseData.Instance.GenericResponse(await _categoryRepo.Update(deleteModel)));
        }
    }
}