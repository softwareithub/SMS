using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.FrontOffice;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.FrontOffice
{
    public class VistorBookDetailController : Controller
    {
        private readonly IGenericRepository<VisitorBook, int> _visitorBookRepo;

        public VistorBookDetailController(IGenericRepository<VisitorBook, int> visitorRepo)
        {
            _visitorBookRepo = visitorRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            var model = await _visitorBookRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/FrontOffice/_VisitorBookPartial.cshtml",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VisitorBook book)
        {
            if (book.Id == 0)
            {
                var response = await _visitorBookRepo.CreateEntity(book);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<VisitorBook>(book, 1);
                var response = await _visitorBookRepo.Update(updateModel);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }

        public async Task<IActionResult> GetList()
        {
            var models = await _visitorBookRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/FrontOffice/_VisitorListPartial.cshtml", models);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _visitorBookRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<VisitorBook>(model, 1);
            await _visitorBookRepo.CreateNewContext();
            var response = await _visitorBookRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}