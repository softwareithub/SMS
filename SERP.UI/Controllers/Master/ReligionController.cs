using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Master
{
    public class ReligionController : Controller
    {
        private readonly IGenericRepository<ReligionMaster, int> _IReligionMaster;

        public ReligionController(IGenericRepository<ReligionMaster, int> religionMaster)
        {
            _IReligionMaster = religionMaster;
        }
        public async Task<IActionResult> CreateReligion(int id=0)
        {
            if(id==0)
                return await Task.Run(()=> PartialView("~/Views/ReligionMaster/_CreateReligionPartial.cshtml")) ;

            var modal =await _IReligionMaster.GetSingle(x => x.Id == id);
            return  PartialView("~/Views/ReligionMaster/_CreateReligionPartial.cshtml", modal);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReligionMaster modal)
        {
            if (modal.Id == 0)
            {
                var result = await _IReligionMaster.CreateEntity(modal);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                var result = await _IReligionMaster.Update(modal);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
           
        }
        public async Task<IActionResult> GetReligionMaster()
        {
            var result =await _IReligionMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/ReligionMaster/_ReligionListPartial.cshtml", result);
        }

        public async Task<IActionResult>DeleteReligion(int id)
        {
            var model = await _IReligionMaster.GetSingle(x => x.Id == id);
            await _IReligionMaster.CreateNewContext();
            model.IsActive = 0; model.IsDeleted = 1;
            model.UpdatedBy = 1; model.UpdatedDate = DateTime.Now.Date;
            var result = await _IReligionMaster.Delete(model);
            return Json(ResponseData.Instance.GenericResponse(result));

        }
    }
}