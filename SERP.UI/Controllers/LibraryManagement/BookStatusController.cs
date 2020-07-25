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
    public class BookStatusController : Controller
    {
        private readonly IGenericRepository<BookStatusModel, int> _bookStatusRepo;
        public BookStatusController(IGenericRepository<BookStatusModel, int> bookStatusRepo)
        {
            _bookStatusRepo = bookStatusRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var model = await _bookStatusRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/LibraryManagement/_BookStatusPartial.cshtml",model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateStatus(BookStatusModel model)
        {
            if(model.Id>0)
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<BookStatusModel>(model, 1);
                return Json(ResponseData.Instance.GenericResponse(await _bookStatusRepo.Update(updateModel)));
            }
            return Json(ResponseData.Instance.GenericResponse(await _bookStatusRepo.CreateEntity(model)));
        }

        public async Task<IActionResult> StatusList()
        {
            return PartialView("~/Views/LibraryManagement/BookStatusList.cshtml",await _bookStatusRepo.GetList(x => x.IsActive == 1));
        }

        public async Task<IActionResult>Delete(int id)
        {
            var model = await _bookStatusRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<BookStatusModel>(model, 1);
            await _bookStatusRepo.CreateNewContext();
            return Json(ResponseData.Instance.GenericResponse(await _bookStatusRepo.Update(deleteModel)));
        }
    }
}