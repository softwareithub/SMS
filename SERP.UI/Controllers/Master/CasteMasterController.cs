using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Master
{
    public class CasteMasterController : Controller
    {
        private readonly IGenericRepository<CasteMaster, int> _ICasteRespo;

        public CasteMasterController(IGenericRepository<CasteMaster, int> casteRepo)
        {
            _ICasteRespo = casteRepo;
        }
        public async Task<IActionResult> CasteMaster(int id=0)
        {
            if(id==0)
                return await Task.Run(() => PartialView("~/Views/CasteMaster/_CasteMasterPartial.cshtml"));

            return PartialView("~/Views/CasteMaster/_CasteMasterPartial.cshtml",await _ICasteRespo.GetSingle(x=>x.Id==id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCaste(CasteMaster model)
        {
            model.IsActive = 1; model.IsDeleted = 0;
            var result= model.Id == 0 ?await  InsertCaste(model) :await  UpdateCaste(model);
            return Json(result);
        }

        public async Task<IActionResult> GetCasteList()
        {
            var result =await _ICasteRespo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/CasteMaster/_CasteMasterList.cshtml", result);
        }

        public async Task<IActionResult> DeleteCaste(int id)
        {
            var model = await _ICasteRespo.GetSingle(x => x.Id == id);
            await _ICasteRespo.CreateNewContext();
            model.IsActive = 0; model.IsDeleted = 1; model.UpdatedBy = 1; model.UpdatedDate = DateTime.Now.Date;
            var deleteResult = await _ICasteRespo.Delete(model);
            return Json(ResponseData.Instance.GenericResponse(deleteResult));
        }

        #region Private
        public async Task<string> InsertCaste(CasteMaster model)
        {
            model.CreatedBy = 1;model.CreatedDate = DateTime.Now.Date;
            var result =await _ICasteRespo.CreateEntity(model);
            return ResponseData.Instance.GenericResponse(result);
        }
        public async Task<string> UpdateCaste(CasteMaster model)
        {
            model.UpdatedBy = 1; model.UpdatedDate = DateTime.Now.Date;
            var result = await _ICasteRespo.Update(model);
            return ResponseData.Instance.GenericResponse(result);
        }
        #endregion

    }
}